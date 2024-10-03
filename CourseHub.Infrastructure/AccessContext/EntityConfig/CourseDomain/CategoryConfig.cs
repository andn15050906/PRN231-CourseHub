using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Infrastructure.AccessContext.Shared;
using CourseHub.Infrastructure.AccessContext.Shared.DbSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.AccessContext.EntityConfig.CourseDomain;

internal class CategoryConfig : SqlServerEntityConfiguration<Category>
{
    protected override Dictionary<Expression<Func<Category, object?>>, string> Columns => new()
    {
        { _ => _.Path, VARCHAR255 },
        { _ => _.Title, VARCHAR100 },
        { _ => _.Description, NVARCHAR1000 },
        // IsLeaf
    };

    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable(RelationsConfig.CATEGORY)
            .SetColumnsTypes(Columns)
            .SetUnique(_ => _.Path, _ => _.Title, _ => _.Description);
    }
}
