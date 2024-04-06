using Domain.SprintAggregation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EFCore.SprintRepository
{
    public class SprintEntityTypeConfiguration : IEntityTypeConfiguration<Sprint>
    {
        public void Configure(EntityTypeBuilder<Sprint> builder)
        {
            builder.ToTable(nameof(ModuleDbContext.Sprints));
            builder.HasKey(x => x.Id);
        }
    }
}
