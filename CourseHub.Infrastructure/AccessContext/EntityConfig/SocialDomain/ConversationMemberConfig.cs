using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.SocialDomain;

internal class ConversationMemberConfig : SqlServerEntityConfiguration<ConversationMember>
{
    protected override Dictionary<Expression<Func<ConversationMember, object?>>, string> Columns => new()
    {
        // IsAdmin
        // LastVisit
    };

    public override void Configure(EntityTypeBuilder<ConversationMember> builder)
    {
        builder
            .ToTable(RelationsConfig.CONVERSATION_MEMBER)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .HasKey(_ => new { _.CreatorId, _.ConversationId });
    }
}
