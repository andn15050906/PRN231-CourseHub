using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class EnrollmentConfig : SqlServerEntityConfiguration<Enrollment>
{
    protected override Dictionary<Expression<Func<Enrollment, object?>>, string> Columns => new()
    {
        // Status below
    };

    public override void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder
            .ToTable(RelationsConfig.ENROLLMENT)
            .SetEnumParsing(_ => _.Status)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .HasKey(_ => new { _.CreatorId, _.CourseId });
    }
}
