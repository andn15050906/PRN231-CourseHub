using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CommonDomain;

internal class CommentConfig : SqlServerEntityConfiguration<Comment>
{
    protected override Dictionary<Expression<Func<Comment, object?>>, string> Columns => new()
    {
        { _ => _.Content, NVARCHAR500 }
        // Status below
        // SourceType below
    };

    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .ToTable(RelationsConfig.COMMENT)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.Status).SetEnumParsing(_ => _.SourceType)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

        builder.OwnsMany(_ => _.Medias, media =>
        {
            media.Property(_ => _.Type).HasConversion(_ => _.ToString(), _ => (CommentMediaType)Enum.Parse(typeof(CommentMediaType), _));
            media.Property(_ => _.Url).HasColumnType(VARCHAR255);
        });
    }
}
