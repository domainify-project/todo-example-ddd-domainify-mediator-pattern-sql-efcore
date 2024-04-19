using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.ProjectSetting
{
    public class SprintViewModel : ViewModel, IModifiedViewModel, IDeletableViewModel
    {
        public string Id { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        public string Name { get; set; } = string.Empty;

        [Display(Name = "Project")]
        public string ProjectName { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public SprintViewModel(Sprint sprint)
        {
            ModifiedDate = sprint.ModifiedDate;
            IsDeleted = sprint.IsDeleted;
            Id = sprint.Id!;
            Name = sprint.Name;
            StartDate = sprint.StartDate;
            EndDate = sprint.EndDate;
        }
    }
}
