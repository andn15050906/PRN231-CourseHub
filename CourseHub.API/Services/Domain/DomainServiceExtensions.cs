using CourseHub.API.Helpers.AppStart;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Services.Domain.AssignmentServices;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;
using CourseHub.Core.Services.Domain.CommonServices;
using CourseHub.Core.Services.Domain.CommonServices.Contracts;
using CourseHub.Core.Services.Domain.CourseServices;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Services.Domain.PaymentServices;
using CourseHub.Core.Services.Domain.PaymentServices.Contracts;
using CourseHub.Core.Services.Domain.SocialServices;
using CourseHub.Core.Services.Domain.SocialServices.Contracts;
using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.Core.Services.Domain.UserServices.Contracts;
using CourseHub.Infrastructure;
using CourseHub.Infrastructure.AccessContext;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.API.Services.Domain;

public static class DomainServiceExtensions
{
    private const byte RETRY_COUNT = 3;
    /*private const bool IS_SENSITIVE_DATA_LOGGING_ENABLED = true;
    private const bool IS_DETAILED_ERRORS_ENABLED = true;*/

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        string connectionString = Configurer.GetContextConnectionString();

        services.AddDbContextPool<Context>(options =>
            options.UseSqlServer(connectionString, act => act.EnableRetryOnFailure(RETRY_COUNT))
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();



        services
            .AddScoped<IUserService, UserService>()

            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IInstructorService, InstructorService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<ILectureService, LectureService>()
            .AddScoped<IEnrollmentService, EnrollmentService>()
            .AddScoped<ICourseCouponService, CourseCouponService>()
            .AddScoped<ICourseReviewService, CourseReviewService>()

            .AddScoped<INotificationService, NotificationService>()
            .AddScoped<ICommentService, CommentService>()
            
            .AddScoped<IBillService, BillService>()
            
            .AddScoped<IAssignmentService, AssignmentService>()
            .AddScoped<IMcqQuestionService, McqQuestionService>()
            .AddScoped<ISubmissionService, SubmissionService>()
            
            .AddScoped<IChatMessageService, ChatMessageService>()
            .AddScoped<IConversationService, ConversationService>();



        ExecuteColdQuery(connectionString);
        ExecuteColdQuery(connectionString);

        return services;
    }

    private static void ExecuteColdQuery(string connectionString)
    {
        using var context = new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString).Options);
        context.Set<User>().FirstOrDefault();
    }
}
