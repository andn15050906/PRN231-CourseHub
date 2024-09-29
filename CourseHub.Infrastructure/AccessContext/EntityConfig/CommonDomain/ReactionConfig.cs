using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CommonDomain;

internal class ReactionConfig : SqlServerEntityConfiguration<Reaction>
{
    protected override Dictionary<Expression<Func<Reaction, object?>>, string> Columns => new()
    {
        { _ => _.Content, VARCHAR10 },
        // SourceType below
    };

    public override void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder
            .ToTable(RelationsConfig.REACTION)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.SourceType)
            .HasKey(_ => new { _.CreatorId, _.SourceEntityId });
    }
}
