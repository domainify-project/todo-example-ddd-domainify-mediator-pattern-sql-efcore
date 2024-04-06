using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    public class ProjectConfiguration : IEntityTypeConfiguration<ProjectModel>
    {
        public void Configure(EntityTypeBuilder<ProjectModel> builder)
        {
            builder.ToTable(nameof(TodoDbContext.Projects));
            builder.HasKey(x => x.Id);

            builder.HasMany(e => e.Sprints)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
