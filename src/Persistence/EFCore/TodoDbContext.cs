using Microsoft.EntityFrameworkCore;
 
namespace Persistence
{
    public class TodoDbContext : DbContext
    {
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<SprintModel> Sprints { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
