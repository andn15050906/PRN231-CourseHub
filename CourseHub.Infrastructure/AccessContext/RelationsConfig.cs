namespace CourseHub.Infrastructure.AccessContext;

internal static class RelationsConfig
{
    internal const string USER = "Users";



    internal const string COMMENT = "Comments";
    internal const string REACTION = "Reactions";
    internal const string NOTIFICATION = "Notifications";



    internal const string CATEGORY = "Categories";
    internal const string COURSE = "Courses";
    internal const string COURSE_COUPON = "CourseCoupons";
    internal const string COURSE_META = "CourseMetas";
    internal const string COURSE_REVIEW = "CourseReviews";
    internal const string ENROLLMENT = "Enrollments";
    internal const string INSTRUCTOR = "Instructors";
    internal const string LECTURE = "Lectures";
    internal const string SECTION = "Sections";
    internal const string COURSE_CATEGORY = "Course_Category";



    internal const string PAYMENT_ACCOUNT = "PaymentAccounts";
    internal const string BILL = "Bills";



    internal const string ASSIGNMENT = "Assignments";
    internal const string MCQ_QUESTION = "McqQuestions";
    internal const string MCQ_CHOICE = "McqChoices";
    internal const string SUBMISSION = "Submissions";



    internal const string CONVERSATION = "Conversations";
    internal const string CONVERSATION_MEMBER = "ConversationMembers";
    internal const string CHAT_MESSAGE = "ChatMessages";
    internal const string ARTICLE = "Articles";



    internal const string TRIGGER_onCourseInsertDelete = "onCourseInsertDelete";
    internal const string TRIGGER_onLectureInsertDelete = "onLectureInsertDelete";
    internal const string TRIGGER_onEnrollmentInsertDelete = "onEnrollmentInsertDelete";
    internal const string TRIGGER_onCourseReviewInsertDelete = "onCourseReviewInsertDelete";
}
