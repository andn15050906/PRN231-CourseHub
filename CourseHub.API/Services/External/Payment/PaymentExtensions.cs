using CourseHub.API.Helpers.AppStart;

namespace CourseHub.API.Services.External.Payment;

public static class PaymentExtensions
{
    public static IServiceCollection AddPaymentService(this IServiceCollection services)
    {
        PaymentOptions config = Configurer.GetPaymentOptions();

        services.Configure<PaymentOptions>(options =>
        {
            options.TmnCode = config.TmnCode;
            options.HashSecret = config.HashSecret;
            options.Url = config.Url;
        });

        return services;
    }
}
