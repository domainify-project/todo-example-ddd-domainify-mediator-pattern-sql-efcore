using Domain.Properties;
using Domainify;
using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.Task
{
    public class TaskViewModel : ViewModel, IModifiedViewModel, IDeletableViewModel
    {
        public string Id { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Project")]
        public string ProjectName { get; set; }
 
        [Display(Name = "Sprint")]
        public string? SprintName { get; set; }
        public TaskStatus Status { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public string Description { get; set; }

        public TaskViewModel(Task task,
            string? projectName = "", string? sprintName = null)
        {
            ModifiedDate = task.ModifiedDate;
            IsDeleted = task.IsDeleted;
            Id = task.Id!;
            ProjectName = projectName!;
            SprintName = sprintName;
            Status = task.Status;
            StatusName = EnumHelper.GetEnumMemberResourceValue(
                Resource.ResourceManager, task.Status!);
            Description = task.Description;
        }
    }
}
