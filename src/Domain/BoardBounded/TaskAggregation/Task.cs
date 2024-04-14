using Domain.Properties;
using Domainify;
using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.TaskAggregation
{
    public class Task : Entity<Task, string>, IAggregateRoot
    {
        public double Version { get; set; }

        [MaxLengthShouldBe(1000)]
        [StringLength(1000)]
        [Required(AllowEmptyStrings = false)]
        public string Description { get; protected set; } = string.Empty;

        public TaskStatus Status { get; protected set; }
        public static Task New() { return new Task(); }

        public Task()
        {
            // The version field is used for persisting in a nosql database like MongoDB
            //Version = 1.0;
        }
        public static Task NewInstance()
        {
            return new Task();
        }

        public Task SetDescription(string value)
        {
            Description = value;

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
                ProjectName = projectName!,
                SprintName = sprintName,
                Status = Status,
                StatusName = EnumHelper.GetEnumMemberResourceValue<TaskStatus>(
                    Resource.ResourceManager, Status!),
                Description = Description
            };
 
            return viewModel;
        }
    }
}
