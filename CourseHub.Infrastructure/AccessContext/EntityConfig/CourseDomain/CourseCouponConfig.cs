using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class CourseCouponConfig : SqlServerEntityConfiguration<CourseCoupon>
{
    protected override Dictionary<Expression<Func<CourseCoupon, object?>>, string> Columns => new()
    {
        { _ => _.Code, VARCHAR10 },
        // Discount
        // ExpiryDate
        // IsUsed
    };

    public override void Configure(EntityTypeBuilder<CourseCoupon> builder)
    {
        builder
            .ToTable(RelationsConfig.COURSE_COUPON)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
    }
}
