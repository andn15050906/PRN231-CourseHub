using CourseHub.Core.Entities.PaymentDomain;
using CourseHub.Core.Interfaces.Repositories.PaymentRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.Services.Mappers.PaymentMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.PaymentRepos;

public class BillRepository : BaseRepository<Bill>, IBillRepository
{
    public BillRepository(DbContext context) : base(context)
    {
    }

    public IPagingQuery<Bill, BillModel> GetPagingQuery(Expression<Func<Bill, bool>>? predicate, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<BillModel>(
            BillMapperProfile.ModelConfig, predicate, pageIndex, pageSize,
            includeExpressions: _ => _.Creator
        );
    }
}
