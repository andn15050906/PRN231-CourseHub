namespace CourseHub.API.Services.External.Payment;

public class PaymentOptions
{
    public string TmnCode { get; set; }
    public string HashSecret { get; set; }
    public string Url { get; set; }
}
