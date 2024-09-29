using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.SocialDomain;

internal class ConversationConfig : SqlServerEntityConfiguration<Conversation>
{
    protected override Dictionary<Expression<Func<Conversation, object?>>, string> Columns => new()
    {
        { _ => _.Title, NVARCHAR45 },
        // IsPrivate
        { _ => _.AvatarUrl, NVARCHAR255 }
    };

    public override void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder
            .ToTable(RelationsConfig.CONVERSATION)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder
            .HasMany(_ => _.Members).WithOne(_ => _.Conversation).OnDelete(DeleteBehavior.NoAction);
    }
}
