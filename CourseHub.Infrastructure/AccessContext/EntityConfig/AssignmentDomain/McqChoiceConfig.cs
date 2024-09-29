using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.AssignmentDomain;

internal class McqChoiceConfig : SqlServerEntityConfiguration<McqChoice>
{
    protected override Dictionary<Expression<Func<McqChoice, object?>>, string> Columns => new()
    {
        { _ => _.Content, NVARCHAR255 }
        // IsCorrect
    };

    public override void Configure(EntityTypeBuilder<McqChoice> builder)
    {
        builder
            .ToTable(RelationsConfig.MCQ_CHOICE)
            .SetColumnsTypes(Columns);
    }
}
