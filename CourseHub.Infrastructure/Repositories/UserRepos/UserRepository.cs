using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Interfaces.Repositories.UserRepos;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.Services.Mappers.UserMappers;
using CourseHub.Infrastructure.AccessContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.UserRepos;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(Context context) : base(context) { }



    public async Task<UserModel?> GetAsync(Guid id)
    {
        return await DbSet
            .Where(_ => _.Id == id)
            .ProjectTo<UserModel>(UserMapperProfile.ModelConfig)
            .FirstOrDefaultAsync();
    }

    public async Task<UserFullModel?> GetFullAsync(Guid id)
    {
        return await DbSet
            .Where(_ => _.Id == id)
            .ProjectTo<UserFullModel>(UserMapperProfile.FullModelConfig)
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserOverviewModel>> GetOverviewAsync(List<Guid> ids)
    {
        return await DbSet
            .Where(_ => ids.Contains(_.Id))
            .ProjectTo<UserOverviewModel>(UserMapperProfile.OverviewModelConfig)
            .ToListAsync();
    }

    public IPagingQuery<User, UserModel> GetPagingQuery(Expression<Func<User, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<UserModel>(UserMapperProfile.ModelConfig, whereExpression, pageIndex, pageSize);
    }

    public async Task<List<UserMinModel>> GetMinAsync(List<Guid> ids)
    {
        return await DbSet
            .Where(_ => ids.Contains(_.Id))
            .ProjectTo<UserMinModel>(UserMapperProfile.MinModelConfig)
            .ToListAsync();
    }

    public async Task<Guid> GetIdByProviderKey(string key)
    {
        return await DbSet.Where(_ => _.ProviderKey == key).Take(1).Select(_ => _.Id).FirstOrDefaultAsync();
    }



    public async Task<bool> EmailExisted(string email) => await Any(_ => _.Email == email);

    public async Task<bool> UserNameExisted(string userName) => await Any(_ => _.UserName == userName);

    public async Task<User?> FindByEmail(string email) => await DbSet.FirstOrDefaultAsync(_ => _.Email == email);

    public async Task<User?> FindByUserName(string userName) => await DbSet.FirstOrDefaultAsync(_ => _.UserName == userName);



    public async Task<bool> UpdateAsInstructor(Guid id, Guid instructorId)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity is null)
            return false;
        entity.Role = Role.Instructor;
        entity.InstructorId = instructorId;
        return true;
    }






    public async Task<List<UserModel>> GetAllAsync()
    {
        return await DbSet
            .ProjectTo<UserModel>(UserMapperProfile.ModelConfig)
            .ToListAsync();
    }

    public async Task<List<UserMinModel>> GetAllMinAsync()
    {
        return await DbSet
            .ProjectTo<UserMinModel>(UserMapperProfile.MinModelConfig)
            .ToListAsync();
    }
}
