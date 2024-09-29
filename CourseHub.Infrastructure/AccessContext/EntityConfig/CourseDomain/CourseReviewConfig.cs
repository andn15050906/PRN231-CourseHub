using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class CourseReviewConfig : SqlServerEntityConfiguration<CourseReview>
{
    protected override Dictionary<Expression<Func<CourseReview, object?>>, string> Columns => new()
    {
        { _ => _.Content, NVARCHAR500 },
        // Rating
    };

    public override void Configure(EntityTypeBuilder<CourseReview> builder)
    {
        builder
            .ToTable(RelationsConfig.COURSE_REVIEW)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
    }
}
