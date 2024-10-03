using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CourseHub.Core.Entities.Contracts;

namespace CourseHub.Infrastructure.AccessContext.Shared.DbSupport;

internal abstract class PostgreEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : DomainObject
{
    protected const string VARCHAR15 = "VARCHAR(15)";
    protected const string VARCHAR20 = "VARCHAR(20)";
    protected const string VARCHAR45 = "VARCHAR(45)";
    protected const string VARCHAR100 = "VARCHAR(100)";
    protected const string VARCHAR255 = "VARCHAR(255)";
    protected const string VARCHAR1000 = "VARCHAR(1000)";

    protected const string DATE = "DATE";
    protected const string TIMESTAMP = "TIMESTAMP";

    protected const string GEN_UUID = "gen_random_uuid()";

    public abstract void Configure(EntityTypeBuilder<T> builder);

    protected abstract Dictionary<Expression<Func<T, object?>>, string> Columns { get; }
}
