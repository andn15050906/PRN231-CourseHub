using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.PaymentServices;
using CourseHub.UI.Services.Contracts.SocialServices;
using CourseHub.UI.Services.Contracts.UserServices;
using CourseHub.UI.Services.Implementations.AssignmentServices;
using CourseHub.UI.Services.Implementations.CommonServices;
using CourseHub.UI.Services.Implementations.CourseServices;
using CourseHub.UI.Services.Implementations.PaymentServices;
using CourseHub.UI.Services.Implementations.SocialServices;
using CourseHub.UI.Services.Implementations.UserServices;

namespace CourseHub.UI.Services;

public static class ApiServicesExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        ApiClientOptions options = Configurer.GetApiClientOptions();

        void config(HttpClient client) => client.BaseAddress = new Uri(options.ApiServerPath);
        services.AddTransient<RefreshTokenHandler>();

        services
            .AddDomainService<IUserApiService, UserApiService>(config)

            .AddDomainService<INotificationApiService, NotificationApiService>(config)
            .AddDomainService<ICommentApiService, CommentApiService>(config)

            .AddDomainService<IInstructorApiService, InstructorApiService>(config)
            .AddDomainService<ICategoryApiService, CategoryApiService>(config)
            .AddDomainService<ICourseApiService, CourseApiService>(config)
            .AddDomainService<ILectureApiService, LectureApiService>(config)
            .AddDomainService<ICourseReviewApiService, CourseReviewApiService>(config)

            .AddDomainService<IPaymentApiService, PaymentApiService>(config)
            
            .AddDomainService<IConversationApiService, ConversationApiService>(config)
            .AddDomainService<IChatMessageApiService, ChatMessageApiService>(config)
            
            .AddDomainService<IAssignmentApiService, AssignmentApiService>(config)
            .AddDomainService<ISubmissionApiService, SubmissionApiService>(config);

        return services;
    }

    private static IServiceCollection AddDomainService<TService, TImplementation>(this IServiceCollection services, Action<HttpClient> config)
            where TService : class
            where TImplementation : class, TService
    {
        services.AddScoped<TService, TImplementation>();
        services.AddHttpClient<TService, TImplementation>(config).AddHttpMessageHandler<RefreshTokenHandler>();
        return services;
    }
}
