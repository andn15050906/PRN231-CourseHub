﻿using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.Entities.CommonDomain;

#pragma warning disable CS8618

public class Comment : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public CommentStatus Status { get; set; }
    public CommentSourceEntityType SourceType { get; private set; }

    // FKs
    public Guid? ParentId { get; set; }
    public Guid? LectureId { get; set; }
    public Guid? ArticleId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public Comment? Parent { get; private set; }
    public Lecture? Lecture { get; set; }
    public Article? Article { get; set; }
    public ICollection<CommentMedia> Medias { get; private set; }
    public ICollection<Comment> Replies { get; private set; }
    public ICollection<Reaction> Reactions { get; private set; }

#pragma warning restore CS8618

    public Comment()
    {

    }

    public Comment(Guid id, User creator, string content, CommentSourceEntityType sourceType, Guid sourceId, ICollection<CommentMedia>? medias = null)
    {
        Id = id;
        Creator = creator;
        SourceType = sourceType;
        switch (SourceType)
        {
            case CommentSourceEntityType.Comment:
                ParentId = sourceId;
                break;
            case CommentSourceEntityType.Lecture:
                LectureId = sourceId;
                break;
            case CommentSourceEntityType.Article:
                ArticleId = sourceId;
                break;
        }

        Content = content;
        Status = CommentStatus.None;

        if (medias is not null)
            Medias = medias;
    }

    public void SetParent(Comment parent)
    {
        Parent = parent;
    }
}
