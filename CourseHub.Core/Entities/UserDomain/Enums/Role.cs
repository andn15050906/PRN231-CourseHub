namespace CourseHub.Core.Entities.UserDomain.Enums;

public enum Role : byte
{
    Learner,
    Instructor,
    Admin,
    SysAdmin
}

public static class RoleConstants {
    public const string LEARNER = nameof(Role.Learner);
    public const string INSTRUCTOR = nameof(Role.Instructor);
    public const string ADMIN = nameof(Role.Admin);
    public const string SYSADMIN = nameof(Role.SysAdmin);
}