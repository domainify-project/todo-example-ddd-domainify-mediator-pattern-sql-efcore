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
        public Guid? SprintId { get; protected set; }


        [MaxLengthShouldBe(1000)]
        [StringLength(1000)]
        [Required(AllowEmptyStrings = false)]
        public string Description { get; protected set; } = string.Empty;

        public Domain.TaskAggregation.TaskStatus Status { get; protected set; }

        public static TaskModel InstanceOf(
            Domain.TaskAggregation.Task task)
        {
            var dataModel = new TaskModel()
            {
                Id = new Guid(task.Id),
                IsDeleted = task.IsDeleted,
                ModifiedDate = task.ModifiedDate,

                ProjectId = new Guid(task.ProjectId),

                Description = task.Description,
                Status = task.Status,
            };

            if (task.SprintId != null)
                dataModel.SprintId = new Guid(task.SprintId);

            return dataModel;
        }

        public Domain.TaskAggregation.Task ToEntity()
        {
            var task = Domain.TaskAggregation.Task.NewInstance(ProjectId.ToString(), SprintId?.ToString());
            task.SetId(Id!.ToString());
            task.ModifiedDate = ModifiedDate;
            task.IsDeleted = IsDeleted;

            task.SetDescription(Description)
                .SetProjectId(ProjectId.ToString());

            if(SprintId != null)
                task.SetSprintId(SprintId.ToString());

            return task;
        }

        public ProjectModel Project { get; private set; }
        public SprintModel? Sprint { get; private set; }
    }
}
