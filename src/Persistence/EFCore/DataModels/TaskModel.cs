using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

namespace Persistence
{
    public class TaskModel
    {
        [Required]
        public required Guid Id { get; set; }
        public required bool IsDeleted { get; set; } = false;

        public required DateTime ModifiedDate { get; set; }

        public required Guid ProjectId { get; set; }
        public Guid? SprintId { get; set; }


        [MaxLengthShouldBe(1000)]
        [StringLength(1000)]
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; } = string.Empty;

        public Domain.Task.TaskStatus Status { get; set; }
        public static TaskModel InstanceOf(
            Domain.Task.Task task, string projectId, string? sprintId)
        {
            var dataModel = new TaskModel()
            {
                Id = Guid.TryParse(task.Id, out var taskId) ? taskId : Guid.NewGuid(),
                IsDeleted = task.IsDeleted,
                ModifiedDate = task.ModifiedDate,

                ProjectId = new Guid(projectId),

                Description = task.Description,
                Status = task.Status,
            };

            dataModel.SprintId = Guid.TryParse(sprintId, out var spId) ? spId : null;

            return dataModel;
        }

        public Domain.Task.Task ToEntity()
        {
            var task = Domain.Task.Task.NewInstance();
            task.SetId(Id!.ToString());
            task.ModifiedDate = ModifiedDate;
            task.IsDeleted = IsDeleted;

            task.SetStatus(Status);
            task.SetDescription(Description);
            return task;
        }

        public ProjectModel Project { get; private set; }
        public SprintModel? Sprint { get; private set; }
    }
}
