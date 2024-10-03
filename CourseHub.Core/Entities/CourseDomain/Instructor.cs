using CourseHub.Core.Helpers.Messaging.Messages;

namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

/// <summary>
/// Instructor info, does not extend User
/// </summary>
public class Instructor : TimeAuditedEntity
{
    // Attributes
    public string Intro { get; set; }
    public string Experience { get; set; }
    // public string Qualifications { get; set; }
    public long Balance { get; set; }
    public byte CourseCount { get; set; }

    // FKs
    public Guid CreatorId { get; set; }
     
    // Navigations
    public User? Creator { get; set; }
    public ICollection<Course> Courses { get; private set; }



    public Instructor()
    {

    }

    public Instructor(Guid id, Guid creatorId, string intro, string experience)
    {
        Id = id;
        Intro = intro;
        Experience = experience;
        CreatorId = creatorId;
    }

#pragma warning restore CS8618

    public void Withdraw(long amount)
    {
        Balance -= amount;
        if (Balance < 0)
            throw new Exception(CourseDomainMessages.INVALID_AMOUNT);
    }
}
