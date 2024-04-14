using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.TaskAggregation
{
    public class TaskViewModel : ViewModel, IModifiedViewModel, IDeletableViewModel
    {
        public string Id { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Project")]
        public required string ProjectName { get; set; }
 
        [Display(Name = "Sprint")]
        public string? SprintName { get; set; }
        public TaskStatus Status { get; set; }
        [Display(Name = "Status")]
        public required string StatusName { get; set; }

        public required string Description { get; set; }
    }
}
