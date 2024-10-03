using AutoMapper;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Interfaces.Authentication;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.Contracts;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using CourseHub.Core.Services.Storage;
using CourseHub.Core.Services.Storage.Utils;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace CourseHub.Core.Services.Domain.UserServices;

public class UserService : DomainService, IUserService
{
    private const byte MAX_ACCESS_FAILED_COUNT = 5;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }






    public async Task<ServiceResult<UserFullModel>> GetFullAsync(Guid id)
    {
        var result = await _uow.UserRepo.GetFullAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<UserModel>> GetAsync(Guid id)
    {
        var result = await _uow.UserRepo.GetAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<PagedResult<UserModel>>> GetPagedAsync(QueryUserDto dto)
    {
        var query = _uow.UserRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);
        var result = await query.ExecuteWithOrderBy(_ => _.UserName);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<UserOverviewModel>>> GetOverviewAsync(List<Guid> ids)
    {
        var result = await _uow.UserRepo.GetOverviewAsync(ids);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<UserMinModel>>> GetMinAsync(List<Guid> ids)
    {
        var result = await _uow.UserRepo.GetMinAsync(ids);
        return ToQueryResult(result);
    }






    /// <summary>
    /// For Learner's Registration
    /// </summary>
    public async Task<ServiceResult<string>> CreateAsync(CreateUserDto dto)
    {
        if (await _uow.UserRepo.EmailExisted(dto.Email))
            return Conflict<string>(UserDomainMessages.CONFLICT_EMAIL);
        if (await _uow.UserRepo.UserNameExisted(dto.UserName))
            return Conflict<string>(UserDomainMessages.CONFLICT_USERNAME);

        User newUser = Adapt(dto, Role.Learner);
        try
        {
            await _uow.UserRepo.Insert(newUser);
            await _uow.CommitAsync();
            return Created(newUser.Token);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError<string>(string.Empty);
        }
    }

    public async Task<ServiceResult<string>> CreateAdminAsync(CreateUserDto dto)
    {
        if (await _uow.UserRepo.EmailExisted(dto.Email))
            return Conflict<string>(UserDomainMessages.CONFLICT_EMAIL);
        if (await _uow.UserRepo.UserNameExisted(dto.UserName))
            return Conflict<string>(UserDomainMessages.CONFLICT_USERNAME);

        User newUser = Adapt(dto, Role.Admin);
        try
        {
            await _uow.UserRepo.Insert(newUser);
            await _uow.CommitAsync();
            return Created(newUser.Token);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError<string>(string.Empty);
        }
    }

    public async Task<ServiceResult<UserFullModel>> UpdateAsync(UpdateUserDto dto, Guid? clientId)
    {
        if (clientId == default)
            return Unauthorized<UserFullModel>();
        User? entity = await _uow.UserRepo.Find(clientId);
        if (entity is null)
            return Unauthorized<UserFullModel>();

        if (dto.CurrentPassword is not null)
        {
            if (dto.NewPassword is null)
                return BadRequest<UserFullModel>(UserDomainMessages.INVALID_NEWPASSWORD_MISSING);
            if (!User.IsMatchPasswords(dto.CurrentPassword, entity.Password))
                return Unauthorized<UserFullModel>(UserDomainMessages.UNAUTHORIZED_PASSWORD);
        }

        try
        {
            await ApplyChanges(dto, entity);
            await _uow.CommitAsync();
            return Ok(_mapper.Map<UserFullModel>(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError<UserFullModel>();
        }
    }



    public async Task<ServiceResult> VerifyAsync(VerifyEmailDto dto)
    {
        var entity = await _uow.UserRepo.FindByEmail(dto.Email);
        if (entity is null)
            return Unauthorized();

        if (entity.Token == dto.Token)
            entity.Approve();
        await _uow.CommitAsync();
        return Ok();
    }

    public async Task<ServiceResult<AuthModel>> SignInAsync(SignInDto dto, ITokenService tokenService)
    {
        User? entity;
        if (dto.UserName is not null)
            entity = await _uow.UserRepo.FindByUserName(dto.UserName);
        else if (dto.Email is not null)
            entity = await _uow.UserRepo.FindByEmail(dto.Email);
        else
            return BadRequest<AuthModel>(UserDomainMessages.INVALID_EMAILPHONE_MISSING);

        if (entity is null)
            return Unauthorized<AuthModel>(UserDomainMessages.UNAUTHORIZED_SIGNIN);
        if (entity.IsNotApproved())
            return Forbidden<AuthModel>(UserDomainMessages.FORBIDDEN_NOT_APPROVED);
        if (entity.AccessFailedCount > MAX_ACCESS_FAILED_COUNT)
            return Forbidden<AuthModel>(UserDomainMessages.FORBIDDEN_FAILED_EXCEED);

        if (!User.IsMatchPasswords(dto.Password, entity.Password))
        {
            entity.IncreaseAccessFailedCount();
            await _uow.CommitAsync();
            return Unauthorized<AuthModel>(UserDomainMessages.UNAUTHORIZED_SIGNIN);
        }

        entity.ResetAccessFailedCount();
        AuthModel authDTO = UpdateToken(tokenService, entity);
        authDTO.User = _mapper.Map<UserFullModel>(entity);
        await _uow.CommitAsync();
        return Ok(authDTO);
    }

    public async Task<ServiceResult<ClaimAuthModel>> ExternalSignInAsync(ClaimsPrincipal claimsPrincipal, Role role)
    {
        if (claimsPrincipal.Identity is null)
            return Unauthorized<ClaimAuthModel>();
        if (role == Role.SysAdmin)
            return Forbidden<ClaimAuthModel>();

        // Requires email
        string? email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        if (email is null)
            return Unauthorized<ClaimAuthModel>();

        User? entity = await _uow.UserRepo.FindByEmail(email);
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is null)
            return Unauthorized<ClaimAuthModel>();
        Claim? providerIdentifier = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (providerIdentifier is null)
            return Unauthorized<ClaimAuthModel>();

        if (entity is null)
        {
            // Register if the user does not have an account
            entity = new User(identity.AuthenticationType!, providerIdentifier.Value, email, identity.Name!, role);
            try
            {
                await _uow.UserRepo.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex.Message);
                return ServerError<ClaimAuthModel>();
            }
        }
        else
        {
            // Update user if the user has an account
            if (entity.Role != role)
                return Unauthorized<ClaimAuthModel>();

            if (entity.LoginProvider != identity.AuthenticationType)
            {
                if (entity.LoginProvider is not null)
                    return Unauthorized<ClaimAuthModel>();
                entity.LoginProvider = identity.AuthenticationType;
            }
            if (entity.ProviderKey != providerIdentifier.Value)
            {
                if (entity.ProviderKey is not null)
                    return Unauthorized<ClaimAuthModel>();
                entity.ProviderKey = providerIdentifier.Value;
            }

            entity.ResetAccessFailedCount();
        }
        await _uow.CommitAsync();

        // SignIn
        if (entity.IsNotApproved())
            return Forbidden<ClaimAuthModel>(UserDomainMessages.FORBIDDEN_NOT_APPROVED);
        var customClaims = identity.Claims.ToList();
        customClaims.Add(new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()));
        customClaims.Add(new Claim(ClaimTypes.Role, entity.Role.ToString()));
        ClaimsPrincipal principle = new(new ClaimsIdentity(customClaims, identity.AuthenticationType));
        return Ok(new ClaimAuthModel(_mapper.Map<UserFullModel>(entity), principle));
    }

    public async Task<ServiceResult<AuthModel>> RefreshAsync(string? accessToken, string? refreshToken, ITokenService tokenService)
    {
        if (accessToken is null || refreshToken is null)
            return Unauthorized<AuthModel>("Missing credentials");
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        // not expired -> invalid
        if (principal is null)
            return Unauthorized<AuthModel>("Invalid access token");

        User? user = await FindByClaims(principal);
        if (user is null)
            return Unauthorized<AuthModel>("Invalid access token");
        if (user.RefreshToken != refreshToken)
            return Unauthorized<AuthModel>("Invalid refresh token");

        AuthModel authDTO = UpdateToken(tokenService, user);
        // no need user
        await _uow.CommitAsync();
        return Ok(authDTO);
    }

    public async Task<ServiceResult<string>> GetPasswordResetTokenAsync(string email)
    {
        User? user = await _uow.UserRepo.FindByEmail(email);
        if (user is null)
            return Unauthorized<string>(UserDomainMessages.INVALID_EMAIL);

        user.GenerateToken();
        await _uow.CommitAsync();
        return Ok(user.Token);
    }

    public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto dto)
    {
        User? user = await _uow.UserRepo.FindByEmail(dto.Email);
        if (user is null || user.Token != dto.Token)
            return BadRequest(UserDomainMessages.INVALID_RESETPASSWORD_ATTEMPT);

        try
        {
            user.SetPassword(dto.NewPassword);
            user.GenerateToken();
            await _uow.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError();
        }
    }

    public async Task<ServiceResult> BlockAsync(Guid userId)
    {
        if (userId == default)
            return BadRequest();
        User? entity = await _uow.UserRepo.Find(userId);
        if (entity is null)
            return BadRequest();

        try
        {
            if (entity.Role < Role.Admin)
                entity.Block();
            await _uow.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError();
        }
    }






    public async Task<ServiceResult> IsValidToken(string email, string token)
    {
        var user = await _uow.UserRepo.FindByEmail(email);

        if (user is null)
            return NotFound();
        if (user.Token != token)
            return NotFound();
        return Ok();
    }

    public async Task ForceCommitAsync()
    {
        await _uow.CommitAsync();
    }

    public async Task<ServiceResult<List<UserMinModel>>> GetAllMinAsync()
    {
        var result = await _uow.UserRepo.GetAllMinAsync();
        return ToQueryResult(result);
    }






    private static User Adapt(CreateUserDto dto, Role role)
    {
        return new User(dto.UserName, dto.Password, dto.Email, role);
    }

    private async Task ApplyChanges(UpdateUserDto dto, User entity)
    {
        if (dto.FullName is not null)
            entity.SetFullName(dto.FullName);
        if (dto.Avatar is not null)
        {
            if (dto.Avatar.File is not null)
                entity.AvatarUrl = await SaveAvatar(dto.Avatar.File, entity.Id);
            else if (dto.Avatar.Url is not null)
                entity.AvatarUrl = dto.Avatar.Url;
        }
        if (dto.DateOfBirth is not null)
            entity.DateOfBirth = (DateTime)dto.DateOfBirth;
        if (dto.Bio is not null)
            entity.Bio = dto.Bio;
        /*if (dto.Phone is not null)
            user.Phone = dto.Phone;*/

        if (dto.CurrentPassword is not null && dto.NewPassword is not null)
            entity.SetPassword(dto.NewPassword);
    }

    private Expression<Func<User, bool>>? GetPredicate(QueryUserDto dto)
    {
        return null;
    }

    private async Task<User?> FindByClaims(ClaimsPrincipal userClaim)
    {
        Guid? id = GetIdentifier(userClaim);
        if (id is null)
            return null;
        return await _uow.UserRepo.Find(id);
    }

    private static Guid? GetIdentifier(ClaimsPrincipal userClaim)
    {
        foreach (Claim claim in userClaim.Claims)
            if (claim.Type == ClaimTypes.NameIdentifier)
                return Guid.TryParse(claim.Value, out Guid result) ? result : null;
        return null;
    }

    private AuthModel UpdateToken(ITokenService tokenService, User user)
    {
        string accessToken = tokenService.GenerateAccessToken(user.Id.ToString(), user.Role.ToString());
        string refreshToken = tokenService.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);
        return new AuthModel(_mapper.Map<UserFullModel>(user), accessToken, refreshToken);
    }

    private async Task<string> SaveAvatar(IFormFile file, Guid userId)
    {
        Stream stream = await new FileConverter().ToJpg(file);
        string path = UserStorage.GetAvatarPath(userId);
        await ServerStorage.SaveFile(stream, path, _logger);
        return path;
    }
}
