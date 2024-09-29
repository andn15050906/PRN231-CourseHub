using CourseHub.Core.Helpers.Messaging.Messages;

namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Category : Entity
{
    public string Path { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsLeaf { get; set; }

    public Category(Guid id)
    {
        Id = id;
    }

    public Category(string title, string description, bool isLeaf, Category? parent = null)
    {
        if (parent is not null && parent.IsLeaf)
            throw new Exception(CourseDomainMessages.INVALID_PARENT);

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsLeaf = isLeaf;
        Path = parent is not null ? $"{parent.Path}_{Id}" : Id.ToString();
    }

    public void SetPath(Category parent)
    {
        Path = $"{parent.Path}_{Id}";
    }
}

#pragma warning restore CS8618