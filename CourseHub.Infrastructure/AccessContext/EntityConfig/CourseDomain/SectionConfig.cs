using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class SectionConfig : SqlServerEntityConfiguration<Section>
{
    protected override Dictionary<Expression<Func<Section, object?>>, string> Columns => new()
    {
        // Index
        { _ => _.Title, NVARCHAR255 }
        // LectureCount
    };

    public override void Configure(EntityTypeBuilder<Section> builder)
    {
        builder
            .ToTable(RelationsConfig.SECTION)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);
    }
}
