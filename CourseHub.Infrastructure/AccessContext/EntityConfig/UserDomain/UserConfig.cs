using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.UserDomain;

internal class UserConfig : SqlServerEntityConfiguration<User>
{
    protected override Dictionary<Expression<Func<User, object?>>, string> Columns => new()
    {
        { _ => _.UserName, VARCHAR45 },
        { _ => _.Password, VARCHAR100 },
        { _ => _.Email, VARCHAR45 },
        { _ => _.FullName, NVARCHAR45 },
        { _ => _.MetaFullName, VARCHAR45 },
        { _ => _.AvatarUrl, VARCHAR100 },
        // Role below
        { _ => _.Token, VARCHAR100 },
        { _ => _.RefreshToken, VARCHAR100 },
        // IsVerified
        // IsApproved
        // AccessFailedCount
        { _ => _.LoginProvider, VARCHAR100 },
        { _ => _.ProviderKey, VARCHAR100 },
        { _ => _.Bio, NVARCHAR1000 },
        // DateOfBirth
        { _ => _.Phone, VARCHAR45 },

        // EnrollmentCount
    };

    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable(RelationsConfig.USER)
            .SetColumnsTypes(Columns)
            .SetEnumParsing(_ => _.Role)
            .SetUnique(_ => _.UserName, _ => _.Email, _ => _.Phone)
            .SetDefaultSQL(_ => _.CreationTime, SQL_GETDATE)
            .SetDefaultSQL(_ => _.LastModificationTime, SQL_GETDATE);

        builder.HasOne(_ => _.Instructor).WithOne(_ => _.Creator).HasForeignKey<Instructor>(_ => _.CreatorId);
    }
}
