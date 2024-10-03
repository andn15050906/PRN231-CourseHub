using CourseHub.Core.Entities.PaymentDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.PaymentDomain;

internal class BillConfig : SqlServerEntityConfiguration<Bill>
{
    protected override Dictionary<Expression<Func<Bill, object?>>, string> Columns => new()
    {
        { _ => _.Action, VARCHAR100 },
        { _ => _.Note, NVARCHAR255 },
        // Amount
        { _ => _.Gateway, VARCHAR20 },
        { _ => _.TransactionId, VARCHAR100 },
        { _ => _.ClientTransactionId, VARCHAR100 },
        { _ => _.Token, VARCHAR100 },
        // IsSuccessful
    };

    public override void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder
            .ToTable(RelationsConfig.BILL)
            .SetColumnsTypes(Columns)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE);
    }
}
