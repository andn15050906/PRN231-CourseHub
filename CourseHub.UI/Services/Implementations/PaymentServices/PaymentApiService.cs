using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.RequestDtos.Payment.BillDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.PaymentServices;

namespace CourseHub.UI.Services.Implementations.PaymentServices;

public class PaymentApiService : IPaymentApiService
{
    private readonly HttpClient _client;

    public PaymentApiService(HttpClient client)
    {
        _client = client;
    }






    public async Task<HttpResponseMessage> GetUrl(CreateBillDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        return await _client.GetAsync($"/api/bills/RedirectLink?{QueryBuilder.Build(dto)}");
    }

    public async Task<PagedResult<BillModel>> GetPagedAsync(QueryBillDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);

        try
        {
            var result = await _client.GetFromJsonAsync<PagedResult<BillModel>>(
                $"api/bills/search?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);
            return result!;
        }
        catch
        {
            return PagedResult<BillModel>.GetEmpty();
        }
    }
}
