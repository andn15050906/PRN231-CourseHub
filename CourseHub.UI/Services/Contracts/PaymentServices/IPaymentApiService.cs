using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.RequestDtos.Payment.BillDtos;

namespace CourseHub.UI.Services.Contracts.PaymentServices;

// Not IBillApiService
public interface IPaymentApiService
{
    Task<HttpResponseMessage> GetUrl(CreateBillDto dto, HttpContext context);
    Task<PagedResult<BillModel>> GetPagedAsync(QueryBillDto dto, HttpContext context);
}
