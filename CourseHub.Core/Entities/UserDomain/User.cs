using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Cryptography;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;

namespace CourseHub.Core.Entities.UserDomain;

public class User : TimeAuditedEntity
{
    // Attributes
    public string UserName { get; set; }
    public string Password { get; private set; }
    public string Email { get; set; }
    public string FullName { get; private set; }
    public string MetaFullName { get; private set; }
    public string AvatarUrl { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public bool IsVerified { get; set; }
    public bool IsApproved { get; private set; }
    public byte AccessFailedCount { get; private set; }
    public string? LoginProvider { get; set; }          // OAuth
    public string? ProviderKey { get; set; }            // OAuth
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }

    public int EnrollmentCount { get; set; }
    public long SystemBalance { get; set; }

    // FKs
    public Guid? InstructorId { get; set; }

    // Navigations
    public Instructor? Instructor { get; set; }
    // public ICollection<Enrollment> Enrollments { get; private set; }
    public ICollection<ConversationMember> ConversationMembers { get; private set; }

#pragma warning disable CS8618

    // For seeding
    public User(Guid id, string fullName, string inputPassword, bool isVerified, bool isApproved)
    {
        Id = id;
        SetFullName(fullName);
        SetPassword(inputPassword);
        SetCreationTime();
        IsVerified = isVerified;
        IsApproved = isApproved;
        AvatarUrl = string.Empty;
        Token = string.Empty;
        RefreshToken = string.Empty;
    }

    public User(string userName, string inputPassword, string email, string fullName, Role role, DateTime dateOfBirth, string phone, Guid id)
    {
        Id = id;
        SetCreationTime();
        UserName = userName;
        SetPassword(inputPassword);
        Email = email;
        SetFullName(fullName);
        SetPassword(inputPassword);
        AvatarUrl = string.Empty;
        Role = role;
        Token = string.Empty;
        RefreshToken = string.Empty;
        IsApproved = true;
        IsVerified = true;
        Bio = string.Empty;
        DateOfBirth = dateOfBirth;
        Phone = phone;
    }

    /// <summary>
    /// Used for registration
    /// </summary>
    public User(string userName, string inputPassword, string email, Role role)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        SetPassword(inputPassword);
        Email = email;
        SetFullName(userName);                  // default Full Name is UserName
        Role = role;
        GenerateToken();
        SetCreationTime();
        AvatarUrl = string.Empty;
        Bio = string.Empty;

        if (Role == Role.Admin)
            IsApproved = true;
        DateOfBirth = new DateTime(2000, 1, 1);
    }

    /// <summary>
    /// Used for external registration
    /// </summary>
    public User(string loginProvider, string providerKey, string? email, string userName, Role role)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        SetFullName(userName);
        SetCreationTime();
        Password = string.Empty;
        AvatarUrl = string.Empty;
        Token = string.Empty;
        RefreshToken = string.Empty;
        LastModificationTime = DateTime.UtcNow;
        Email = email is not null ? email : string.Empty;
        Bio = string.Empty;
        Role = role;
        IsVerified = true;
        // IsApproved
        LoginProvider = loginProvider;
        ProviderKey = providerKey;

        DateOfBirth = new DateTime(2000, 1, 1);
    }

#pragma warning restore CS8618

    public void SetFullName(string fullName)
    {
        FullName = fullName;
        MetaFullName = TextHelper.Normalize(fullName);
    }

    public void SetPassword(string plainTextPassword)
    {
        Password = HashPassword(plainTextPassword);
    }

    public void Approve()
    {
        if (Role == Role.Learner || Role == Role.Instructor)
            IsVerified = true;
        else if (Role == Role.Admin)
            IsApproved = true;
    }

    public void GenerateToken()
    {
        Token = Guid.NewGuid().ToString();
        RefreshToken = string.Empty;
    }

    public void SetRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
        LastModificationTime = DateTime.UtcNow;
    }

    public void IncreaseAccessFailedCount()
    {
        AccessFailedCount++;
    }

    public void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
    }

    public bool IsNotApproved()
    {
        return Role == Role.Admin ? !IsApproved : !IsVerified;
    }

    public void SetInstructor(Guid instructorId)
    {
        if (Role > Role.Learner)
            throw new Exception(UserDomainMessages.FORBIDDEN_NOT_APPROVED);
        Role = Role.Instructor;
        InstructorId = instructorId;
    }

    private void SetCreationTime()
    {
        DateTime now = DateTime.UtcNow;
        CreationTime = now;
        LastModificationTime = now;
    }

    public void Block()
    {
        IsApproved = false;
        AccessFailedCount = 100;
    }

    public bool IsBlocked()
    {
        return AccessFailedCount == 100;
    }






    public static bool IsMatchPasswords(string plainTextPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(plainTextPassword))
            return false;
        return hashedPassword == HashPassword(plainTextPassword);
    }

    private static string HashPassword(string plainTextPassword)
    {
        return SHA256Helper.Hash(plainTextPassword);
    }
}