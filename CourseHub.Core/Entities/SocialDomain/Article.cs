namespace CourseHub.Core.Entities.SocialDomain;

#pragma warning disable CS8618

public class Article : AuditedEntity
{
    // Attributes
    //public string Restriction { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Reaction> Reactions { get; set; }

#pragma warning restore CS8618
}
