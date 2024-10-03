namespace CourseHub.Core.Entities.CommonDomain.Enums;

public enum NotificationType : byte
{
    AdminMessage,

    RequestToBecomeInstructor,
    InstructorResponse,

    ReportGroup,
    GroupAdminReportedGroup,

    ReportCourse,
    InstructorReportedCourse,

    InviteMember,

    RequestWithdrawal
}
