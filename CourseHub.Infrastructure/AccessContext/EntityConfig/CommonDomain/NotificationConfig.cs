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
        { _ => _.Type, NVARCHAR45 },
        { _ => _.Status, NVARCHAR45 }
    };

    public override void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .ToTable(RelationsConfig.NOTIFICATION)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);

        builder.HasOne(_ => _.Receiver).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
