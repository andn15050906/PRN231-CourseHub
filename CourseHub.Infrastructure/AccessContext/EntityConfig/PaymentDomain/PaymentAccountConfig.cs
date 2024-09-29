using CourseHub.Core.Entities.PaymentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.PaymentDomain;

internal class PaymentAccountConfig : SqlServerEntityConfiguration<PaymentAccount>
{
    protected override Dictionary<Expression<Func<PaymentAccount, object?>>, string> Columns => new()
    {
        { _ => _.Gateway, VARCHAR15 },
        { _ => _.AccountNumber, VARCHAR45 },
        { _ => _.AccountHolderName, VARCHAR45 }
    };

    public override void Configure(EntityTypeBuilder<PaymentAccount> builder)
    {
        builder
            .ToTable(RelationsConfig.PAYMENT_ACCOUNT)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
    }
}
