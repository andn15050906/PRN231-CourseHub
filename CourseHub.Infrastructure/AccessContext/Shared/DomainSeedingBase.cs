using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CourseHub.Core.Entities.Contracts;

namespace CourseHub.Infrastructure.AccessContext.Shared;

internal abstract class DomainSeedingBase<T> : IEntityTypeConfiguration<T> where T : DomainObject
{
    internal abstract List<T> seedValues { get; }

    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasData(seedValues);
    }
}
