using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Payment;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.PaymentRepos;

public interface IBillRepository : IRepository<Bill>
{
    IPagingQuery<Bill, BillModel> GetPagingQuery(Expression<Func<Bill, bool>>? predicate, short pageIndex, byte pageSize);
}
