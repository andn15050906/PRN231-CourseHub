using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.Core.Services.Domain.SocialServices.Contracts;
using CourseHub.Core.Services.Storage.Utils;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Domain.SocialServices;

public class ConversationService : DomainService, IConversationService
{
    public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public async Task<ServiceResult<ConversationModel>> Get(Guid id, Guid? client)
    {
        var result = await _uow.ConversationRepo.GetAsync(id);

        /*if (result is null)
            return NotFound<ConversationModel>();
        if (!result.Members.Any(_ => _.CreatorId == client))
            return Unauthorized<ConversationModel>();*/

        return ToQueryResult(result);
    }

    public async Task<ServiceResult<PagedResult<ConversationModel>>> Get(QueryConversationDto dto, Guid? client)
    {
        var query = _uow.ConversationRepo.GetPagingQuery(GetPredicate(dto, client), dto.PageIndex, dto.PageSize);

        PagedResult<ConversationModel> result = await query.ExecuteWithOrderBy(_ => _.CreationTime);

        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<ConversationModel>>> GetMultiple(List<Guid> ids)
    {
        var result = await _uow.ConversationRepo.GetMultipleAsync(ids);

        return ToQueryResult(result);
    }

    public async Task<ServiceResult<PagedResult<ConversationModel>>> GetConversationsOrUsers(QueryConversationDto dto, Guid? client)
    {
        var conversationQuery = _uow.ConversationRepo.GetPagingQuery(GetPredicate(dto, client), dto.PageIndex, dto.PageSize);

        IPagingQuery<User, UserModel>? userQuery = null;
        if (dto.Title is not null)
        {
            userQuery = _uow.UserRepo.GetPagingQuery(_ => _.MetaFullName.Contains(TextHelper.Normalize(dto.Title)), dto.PageIndex, dto.PageSize);
        }

        var conversations = await conversationQuery.ExecuteWithOrderBy(_ => _.CreationTime);
        PagedResult<UserModel> users = dto.Title is not null
            ? await userQuery!.ExecuteWithOrderBy(_ => _.MetaFullName)
            : PagedResult<UserModel>.GetEmpty();


        var privateConversations = users.Items.Select(_ => new ConversationModel
        {
            Id = Guid.Empty,
            CreationTime = default,
            CreatorId = _.Id,
            Title = _.FullName,         /**/
            IsPrivate = true,
            AvatarUrl = _.AvatarUrl
            /*Members*/
        }).ToList();

        PagedResult<ConversationModel> result = new(
            conversations.TotalCount + privateConversations.Count,
            dto.PageIndex,
            dto.PageSize,
            conversations.Items.Union(privateConversations).ToList()
        );

        return ToQueryResult(result);
    }






    public async Task<ServiceResult<Guid>> Create(CreateConversationDto dto, Guid client)
    {
        // request.OtherParticipants -> AllParticipants
        dto.OtherParticipants.Add(client);
        dto.OtherParticipants = dto.OtherParticipants.Distinct().ToList();
        if (dto.OtherParticipants.Count < 2)
            return BadRequest<Guid>(ConversationDomainMessages.INVALID_PARTICIPANTS);

        var entity = await Adapt(dto, client);
        await _uow.ConversationRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created(entity.Id);
    }

    public async Task<ServiceResult> Update(UpdateConversationDto dto, Guid client)
    {
        var entity = await _uow.ConversationRepo.FindWithMembers(dto.Id);
        if (entity is null)
            return BadRequest();

        var clientMember = entity.Members.FirstOrDefault(_ => _.CreatorId == client);
        //...
        if (clientMember is null)
            return Unauthorized();

        try
        {
            await ApplyChanges(dto, entity);
            await _uow.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError();
        }
    }

    public async Task<ServiceResult> Delete(Guid id, Guid client)
    {
        var entity = await _uow.ConversationRepo.FindWithMembers(id);
        if (entity is null)
            return BadRequest();
        var clientMember = entity.Members.FirstOrDefault(_ => _.CreatorId == client);
        if (clientMember is null || !clientMember.IsAdmin)
            return Unauthorized();

        _uow.ConversationRepo.Delete(entity);
        await _uow.CommitAsync();
        return Ok();
    }





    private async Task<Conversation> Adapt(CreateConversationDto _, Guid client)
    {
        Guid id = Guid.NewGuid();

        bool isPrivate = _.OtherParticipants.Count == 2;

        string avatarUrl = string.Empty;
        if (_.Avatar is not null)
        {
            if (_.Avatar.File is not null)
                avatarUrl = await SaveAvatar(_.Avatar.File, id);
            else if (_.Avatar.Url is not null)
                avatarUrl = _.Avatar.Url;
        }

        List<ConversationMember> members = _.OtherParticipants
            .Select(_ => new ConversationMember(id, _, _ == client))
            .ToList();



        // request.OtherParticipants == AllParticipants
        Conversation entity = new(
            id,
            client,
            isPrivate || _.Title is null ? string.Empty : _.Title,
            isPrivate,
            avatarUrl,
            members
        );

        return entity;
    }

    private async Task ApplyChanges(UpdateConversationDto _, Conversation entity)
    {
        //... dto

        if (_.Title is not null)
            entity.Title = _.Title;

        if (_.Avatar is not null)
        {
            string avatarUrl = string.Empty;
            if (_.Avatar.File is not null)
                avatarUrl = await SaveAvatar(_.Avatar.File, entity.Id);
            else if (_.Avatar.Url is not null)
                avatarUrl = _.Avatar.Url;

            entity.AvatarUrl = avatarUrl;
        }
    }






    private Expression<Func<Conversation, bool>>? GetPredicate(QueryConversationDto dto, Guid? client)
    {
        if (dto.ByClient)
            return _ => _.Members.Any(_ => _.CreatorId == client);

        //...
        if (dto.Title is not null)
            return _ => _.Title.Contains(TextHelper.Normalize(dto.Title));

        // Where Contains
        //dto.ConversationIds
        return null;
    }

    private async Task<string> SaveAvatar(IFormFile file, Guid courseId)
    {
        Stream stream = await new FileConverter().ToJpg(file);
        string path = SocialStorage.GetAvatarPath(courseId);
        await ServerStorage.SaveFile(stream, path, _logger);
        return path;
    }
}
