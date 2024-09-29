using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class InstructorConfig : SqlServerEntityConfiguration<Instructor>
{
    protected override Dictionary<Expression<Func<Instructor, object?>>, string> Columns => new()
    {
        { _ => _.Intro, NVARCHAR500 },
        { _ => _.Experience, NVARCHAR1000 }
    };

    public override void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder
            .ToTable(RelationsConfig.INSTRUCTOR)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
    }
}
