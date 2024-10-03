using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CommonDomain;

internal class NotificationConfig : SqlServerEntityConfiguration<Notification>
{
    protected override Dictionary<Expression<Func<Notification, object?>>, string> Columns => new()
    {
        { _ => _.Message, NVARCHAR255 },
        // Type below
        // Status below
    };

    public override void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .ToTable(RelationsConfig.NOTIFICATION)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.Type).SetEnumParsing(_ => _.Status)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.HasOne(_ => _.Receiver).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
