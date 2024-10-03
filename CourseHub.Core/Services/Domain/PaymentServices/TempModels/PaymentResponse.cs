namespace CourseHub.Core.Services.Domain.PaymentServices.TempModels;

public class PaymentResponse
{
    public long Amount { get; set; }
    public string TransactionId { get; set; }
    public string ClientTransactionId { get; set; }
    public string Token { get; set; }
    public bool IsSuccessful { get; set; }
}
