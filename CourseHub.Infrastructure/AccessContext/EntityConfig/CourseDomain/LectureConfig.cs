using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class LectureConfig : SqlServerEntityConfiguration<Lecture>
{
    protected override Dictionary<Expression<Func<Lecture, object?>>, string> Columns => new()
    {
        { _ => _.Title, NVARCHAR255 },
        { _ => _.Content, NVARCHAR3000 }
    };

    public override void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder
            .ToTable(RelationsConfig.LECTURE)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.OwnsMany(_ => _.Materials, material => {
            material.Property(_ => _.Type).HasConversion(_ => _.ToString(), _ => (LectureMaterialType)Enum.Parse(typeof(LectureMaterialType), _));
            material.Property(_ => _.Url).HasColumnType(VARCHAR255);
        });
    }
}
