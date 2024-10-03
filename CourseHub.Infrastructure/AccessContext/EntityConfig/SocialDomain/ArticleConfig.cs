using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.SocialDomain;

internal class ArticleConfig : SqlServerEntityConfiguration<Article>
{
    protected override Dictionary<Expression<Func<Article, object?>>, string> Columns => new()
    {
        //{ _ => _.Restriction, VARCHAR45 },
        { _ => _.Content, NVARCHAR3000 },
        { _ => _.Title, NVARCHAR255 },
        { _ => _.Status, VARCHAR45 },
        // IsCommentDisabled
        // CommentCount
        // ViewCount
    };

    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .ToTable(RelationsConfig.ARTICLE)
            .SetColumnsTypes(Columns)
            .SetUnique(_ => _.Title)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

        builder.OwnsMany(_ => _.Tags, tag =>
        {
            tag.Property(_ => _.Title).HasColumnType(NVARCHAR45);
        });
    }
}
