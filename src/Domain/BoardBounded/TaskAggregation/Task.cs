using Domain.ProjectSettingAggregation;
using Domain.Properties;
using Domainify;
using Domainify.Domain;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Domain.TaskAggregation
{
    public class Task : Entity<Task, string>, IAggregateRoot
    {
        public double Version { get; set; }

        [MaxLengthShouldBe(1000)]
        [StringLength(1000)]
        [Required(AllowEmptyStrings = false)]
        public string Description { get; protected set; } = string.Empty;

        [Required]
        public string ProjectId { get; protected set; }
        public string? SprintId { get; protected set; }
        public TaskStatus Status { get; protected set; }
        public static Task New() { return new Task(); }

        public Task()
        {
            Version = 1.0;
        }
        public static Task NewInstance(
            string projectId,
            string? sprintId = null)
        {
            return new Task()
                .SetProjectId(projectId)
                .SetSprintId(sprintId);
        }

        public Task SetProjectId(string value)
        {
            ProjectId = value;

            return this;
        }
        public Task SetDescription(string value)
        {
            Description = value;

            return this;
        }
        public Task SetSprintId(string? value)
        {
            SprintId = value;

            return this;
        }
        public Task SetStatus(TaskStatus value)
        {
            Status = value;

            return this;
        }
        public static TaskStatus GetTaskStatusDefaultValue()
        {
            return TaskStatus.NotStarted;
        }

        public TaskViewModel ToViewModel(
            string? projectName = "", string? sprintName = null)
        {
            var viewModel = new TaskViewModel()
            {
                ModifiedDate = ModifiedDate,
                IsDeleted = IsDeleted,
                Id = Id!,
                ProjectId = ProjectId,
                SprintId = SprintId!,
                ProjectName = projectName!,
                SprintName = sprintName,
                Status = Status,
                StatusName = EnumHelper.GetEnumMemberResourceValue<TaskStatus>(
                    Resource.ResourceManager, Status!)
            };
 
            return viewModel;
        }
    }
}
