using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.AssignmentDomain;

internal class McqQuestionConfig : SqlServerEntityConfiguration<McqQuestion>
{
    protected override Dictionary<Expression<Func<McqQuestion, object?>>, string> Columns => new()
    {
        { _ => _.Content, NVARCHAR500 }
    };

    public override void Configure(EntityTypeBuilder<McqQuestion> builder)
    {
        builder
            .ToTable(RelationsConfig.MCQ_QUESTION)
            .SetColumnsTypes(Columns);

        builder.HasMany(_ => _.Choices).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
