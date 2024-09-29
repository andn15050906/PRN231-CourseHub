using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.AssignmentDomain;

internal class SubmissionConfig : SqlServerEntityConfiguration<Submission>
{
    protected override Dictionary<Expression<Func<Submission, object?>>, string> Columns => new()
    {
        // Mark
        // TimeSpentInSec
    };

    public override void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder
            .ToTable(RelationsConfig.SUBMISSION)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.OwnsMany(_ => _.Answers, answer =>
        {
            answer.HasOne(_ => _.Submission).WithMany(_ => _.Answers).OnDelete(DeleteBehavior.NoAction);
        });
        builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
