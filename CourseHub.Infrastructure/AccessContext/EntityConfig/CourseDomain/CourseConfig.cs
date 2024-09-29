using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class CourseConfig : SqlServerEntityConfiguration<Course>
{
    protected override Dictionary<Expression<Func<Course, object?>>, string> Columns => new()
    {
        { _ => _.Title, NVARCHAR255 },
        { _ => _.MetaTitle, VARCHAR255 },
        { _ => _.ThumbUrl, NVARCHAR255 },
        { _ => _.Intro, NVARCHAR500 },
        { _ => _.Description, NVARCHAR1000 },
        // Status below
        // Price
        // Discount
        // DiscountExpiry
        // Level below
        { _ => _.Outcomes, NVARCHAR500 },
        { _ => _.Requirements, NVARCHAR500 },
        // LectureCount
        // LearnerCount
        // RatingCount
        // BookmarkCount
    };

    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .ToTable(RelationsConfig.COURSE)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.Status).SetEnumParsing(_ => _.Level)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.OwnsMany(_ => _.Metas, meta => meta.Property(_ => _.Value).HasColumnType(NVARCHAR100));
        builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
