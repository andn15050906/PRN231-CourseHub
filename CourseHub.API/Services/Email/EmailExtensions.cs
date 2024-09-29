using CourseHub.API.Helpers.AppStart;

namespace CourseHub.API.Services.Email;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        EmailOptions config = Configurer.GetEmailOptions();

        services.Configure<EmailOptions>(options =>
        {
            options.SenderMail = config.SenderMail;
            options.SenderPassword = config.SenderPassword;
        });
        services.AddScoped<EmailService>();
        return services;
    }
}
