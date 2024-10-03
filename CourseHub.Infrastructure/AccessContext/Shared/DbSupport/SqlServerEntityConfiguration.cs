using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CourseHub.Core.Entities.Contracts;

namespace CourseHub.Infrastructure.AccessContext.Shared.DbSupport;

internal abstract class SqlServerEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : DomainObject
{
    public const string VARCHAR10 = "VARCHAR(10)";
    public const string VARCHAR15 = "VARCHAR(15)";
    public const string VARCHAR20 = "VARCHAR(20)";
    public const string VARCHAR45 = "VARCHAR(45)";
    public const string VARCHAR100 = "VARCHAR(100)";
    public const string VARCHAR255 = "VARCHAR(255)";
    public const string VARCHAR1000 = "VARCHAR(1000)";

    public const string NVARCHAR45 = "NVARCHAR(45)";
    public const string NVARCHAR100 = "NVARCHAR(100)";
    public const string NVARCHAR255 = "NVARCHAR(255)";
    public const string NVARCHAR500 = "NVARCHAR(500)";
    public const string NVARCHAR1000 = "NVARCHAR(1000)";
    public const string NVARCHAR3000 = "NVARCHAR(3000)";

    public const string DATE = "DATE";
    public const string DATETIME2 = "DATETIME2";            // default for DateTime

    public const string SQL_GETDATE = "GETDATE()";

    public abstract void Configure(EntityTypeBuilder<T> builder);

    protected abstract Dictionary<Expression<Func<T, object?>>, string> Columns { get; }
}
