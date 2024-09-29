using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Cryptography;
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
    public bool IsApproved { get; private set; }        // for Admins
    public byte AccessFailedCount { get; private set; }
    public string? LoginProvider { get; set; }          // OAuth
    public string? ProviderKey { get; set; }            // OAuth
    public string Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }

    public int EnrollmentCount { get; set; }
    //public int SystemBalance { get; set; }
    //public int Point { get; set; }

    // Navigations
    // public ICollection<Enrollment> Enrollments { get; private set; }
    public ICollection<PaymentAccount> PaymentAccounts { get; private set; }
    public ICollection<ConversationMember> ConversationMembers { get; private set; }

#pragma warning disable CS8618

    public User(Guid id)
    {
        Id = id;
    }

    public User(Guid id, string fullName, string inputPassword)
    {
        Id = id;
        SetFullName(fullName);
        SetPassword(inputPassword);
        SetCreationTime();
    }

    /// <summary>
    /// Used for password registration
    /// </summary>
    public User(string userName, string inputPassword)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        SetFullName(userName);                  // default Full Name is UserName
        SetPassword(inputPassword);
        GenerateToken();
        SetCreationTime();
        AvatarUrl = string.Empty;
        Bio = string.Empty;
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

    private void SetCreationTime()
    {
        DateTime now = DateTime.UtcNow;
        CreationTime = now;
        LastModificationTime = now;
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