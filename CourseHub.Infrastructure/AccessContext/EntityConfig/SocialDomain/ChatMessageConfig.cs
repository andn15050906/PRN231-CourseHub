using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.SocialDomain;

internal class ChatMessageConfig : SqlServerEntityConfiguration<ChatMessage>
{
    protected override Dictionary<Expression<Func<ChatMessage, object?>>, string> Columns => new()
    {
        { _ => _.Content, NVARCHAR255 },
        // Status below
    };

    public override void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder
            .ToTable(RelationsConfig.CHAT_MESSAGE)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.Status)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.HasOne(_ => _.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
