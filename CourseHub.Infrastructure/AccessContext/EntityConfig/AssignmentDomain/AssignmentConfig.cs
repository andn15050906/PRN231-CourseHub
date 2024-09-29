using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.AssignmentDomain;

internal class AssignmentConfig : SqlServerEntityConfiguration<Assignment>
{
    protected override Dictionary<Expression<Func<Assignment, object?>>, string> Columns => new()
    {
        { _ => _.Name, NVARCHAR255 },
        // Duration
        // QuestionCount
    };

    public override void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder
            .ToTable(RelationsConfig.ASSIGNMENT)
            .SetColumnsTypes(Columns);
    }
}
