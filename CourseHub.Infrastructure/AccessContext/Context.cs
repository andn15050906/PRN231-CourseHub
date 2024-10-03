using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.AssignmentDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.CommonDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.PaymentDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.SocialDomain;
using CourseHub.Infrastructure.AccessContext.EntityConfig.UserDomain;
using CourseHub.Infrastructure.AccessContext.EntitySeeding.UserDomain;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.AccessContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfiguration(new UserConfig())

                .ApplyConfiguration(new CommentConfig())
                .ApplyConfiguration(new ReactionConfig())
                .ApplyConfiguration(new NotificationConfig())

                .ApplyConfiguration(new InstructorConfig())
                .ApplyConfiguration(new CourseConfig())
                .ApplyConfiguration(new SectionConfig())
                .ApplyConfiguration(new LectureConfig())
                .ApplyConfiguration(new CategoryConfig())
                .ApplyConfiguration(new EnrollmentConfig())
                .ApplyConfiguration(new CourseReviewConfig())
                //.ApplyConfiguration(new CourseCouponConfig())

                .ApplyConfiguration(new BillConfig())

                .ApplyConfiguration(new AssignmentConfig())
                .ApplyConfiguration(new McqQuestionConfig())
                .ApplyConfiguration(new McqChoiceConfig())
                .ApplyConfiguration(new SubmissionConfig())

                .ApplyConfiguration(new ConversationConfig())
                .ApplyConfiguration(new ConversationMemberConfig())
                .ApplyConfiguration(new ChatMessageConfig())
                .ApplyConfiguration(new ArticleConfig());

            builder
                .ApplyConfiguration(new UserSeeding());
        }
    }
}
