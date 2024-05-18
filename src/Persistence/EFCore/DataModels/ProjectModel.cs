
using Domain.ProjectSettingAggregation;
using System.ComponentModel.DataAnnotations;

namespace Persistence
{
    public class ProjectModel
    {
        [Required]
        public required Guid Id { get; set; }
        public required bool IsDeleted { get; set; } = false;

        public required DateTime ModifiedDate { get; set; }

        public required string Name { get; set; }

        public List<Sprint> SprintsList { get; set; } = new List<Sprint>();

        public static ProjectModel InstanceOf(Project project)
        {
            var dataModel = new ProjectModel()
            {
                Id = Guid.TryParse(project.Id, out var projectId) ? projectId : Guid.NewGuid(),
                IsDeleted = project.IsDeleted,
                ModifiedDate = project.ModifiedDate,

                Name = project.Name,
            };
  
            return dataModel;
        }

        public Project ToEntity()
        {
            var project = Project.NewInstance();
            project.SetId(Id.ToString());
            project.ModifiedDate = ModifiedDate;
            project.IsDeleted = IsDeleted;

            project.SetName(Name);
            //project.Sprints = SprintsList;

            return project;
        }

        public ICollection<SprintModel> Sprints { get; }
        public ICollection<TaskModel> Tasks { get; }
    }
}
