using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.PaymentRepos;
using CourseHub.Core.Interfaces.Repositories.SocialRepos;
using CourseHub.Core.Interfaces.Repositories.UserRepos;
using CourseHub.Infrastructure.AccessContext;
using CourseHub.Infrastructure.Repositories.AssignmentRepos;
using CourseHub.Infrastructure.Repositories.CommonRepos;
using CourseHub.Infrastructure.Repositories.CourseRepos;
using CourseHub.Infrastructure.Repositories.PaymentRepos;
using CourseHub.Infrastructure.Repositories.SocialRepos;
using CourseHub.Infrastructure.Repositories.UserRepos;

namespace CourseHub.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }






    private UserRepository? _userRepo;
    public IUserRepository UserRepo { get => _userRepo ??= new UserRepository(_context); }



    private CategoryRepository? _categoryRepo;
    public ICategoryRepository CategoryRepo { get => _categoryRepo ??= new CategoryRepository(_context); }

    private InstructorRepository? _instructorRepo;
    public IInstructorRepository InstructorRepo { get => _instructorRepo ??= new InstructorRepository(_context); }

    private CourseRepository? _courseRepo;
    public ICourseRepository CourseRepo { get => _courseRepo ??= new CourseRepository(_context); }

    private SectionRepository? _sectionRepo;
    public ISectionRepository SectionRepo { get => _sectionRepo ??= new SectionRepository(_context); }

    private LectureRepository _lectureRepo;
    public ILectureRepository LectureRepo { get => _lectureRepo ??= new LectureRepository(_context); }

    private CourseCouponRepository _courseCouponRepo;
    public ICourseCouponRepository CourseCouponRepo { get => _courseCouponRepo ??= new CourseCouponRepository(_context); }

    private CourseReviewRepository _courseReviewRepo;
    public ICourseReviewRepository CourseReviewRepo { get => _courseReviewRepo ??= new CourseReviewRepository(_context); }

    private EnrollmentRepository _enrollmentRepo;
    public IEnrollmentRepository EnrollmentRepo { get => _enrollmentRepo ??= new EnrollmentRepository(_context); }






    private NotificationRepository? _notificationRepo;
    public INotificationRepository NotificationRepo { get => _notificationRepo ??= new NotificationRepository(_context); }

    private CommentRepository? _commentRepo;
    public ICommentRepository CommentRepo { get => _commentRepo ??= new CommentRepository(_context); }

    private ReactionRepository? _reactionRepo;
    public IReactionRepository ReactionRepo { get => _reactionRepo ??= new ReactionRepository(_context); }






    private BillRepository? _billRepo;
    public IBillRepository BillRepo { get => _billRepo ??= new BillRepository(_context); }






    private ConversationRepository _conversationRepo;
    public IConversationRepository ConversationRepo { get => _conversationRepo ??= new ConversationRepository(_context); }

    private ChatMessageRepository _chatMessageRespo;
    public IChatMessageRepository ChatMessageRepo { get => _chatMessageRespo ??= new ChatMessageRepository(_context); }

    public ITagRepository TagRepo => throw new NotImplementedException();






    private AssignmentRepository _assignmentRepo;
    public IAssignmentRepository AssignmentRepo { get => _assignmentRepo ??= new AssignmentRepository(_context); }

    private McqQuestionRepository _mcqQuestionRepo;
    public IMcqQuestionRepository McqQuestionRepo { get => _mcqQuestionRepo ??= new McqQuestionRepository(_context); }

    private McqChoiceRepository _mcqChoiceRepo;
    public IMcqChoiceRepository McqChoiceRepo { get => _mcqChoiceRepo ?? new McqChoiceRepository(_context); }

    private SubmissionRepository _submissionRepo;
    public ISubmissionRepository SubmissionRepo { get => _submissionRepo ?? new SubmissionRepository(_context); }
}
