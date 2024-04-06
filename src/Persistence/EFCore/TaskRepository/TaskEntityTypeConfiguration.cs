using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EFCore.TaskRepository
{
    public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Domain.TaskAggregation.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.TaskAggregation.Task> builder)
        {
            builder.ToTable(nameof(ModuleDbContext.Tasks));
            builder.HasKey(x => x.Id);
        }
    }
}
