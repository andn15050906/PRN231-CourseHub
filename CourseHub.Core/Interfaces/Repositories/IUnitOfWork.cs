using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.PaymentRepos;
using CourseHub.Core.Interfaces.Repositories.SocialRepos;
using CourseHub.Core.Interfaces.Repositories.UserRepos;

namespace CourseHub.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();

    IUserRepository UserRepo { get; }

    ICategoryRepository CategoryRepo { get; }
    IInstructorRepository InstructorRepo { get; }
    ICourseRepository CourseRepo { get; }
    ISectionRepository SectionRepo { get; }
    ICourseCouponRepository CourseCouponRepo { get; }
    ICourseReviewRepository CourseReviewRepo { get; }
    ILectureRepository LectureRepo { get; }
    IEnrollmentRepository EnrollmentRepo { get; }

    INotificationRepository NotificationRepo { get; }
    ICommentRepository CommentRepo { get; }
    IReactionRepository ReactionRepo { get; }

    IBillRepository BillRepo { get; }

    IConversationRepository ConversationRepo { get; }
    IChatMessageRepository ChatMessageRepo { get; }
    ITagRepository TagRepo { get; }

    IAssignmentRepository AssignmentRepo { get; }
    IMcqQuestionRepository McqQuestionRepo { get; }
    IMcqChoiceRepository McqChoiceRepo { get; }
    ISubmissionRepository SubmissionRepo { get; }
}
