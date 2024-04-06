using Domainify.Domain;
using System.ComponentModel.DataAnnotations;
using Domain.Properties;

namespace Domain.ProjectSettingAggregation
{
    public class Sprint : Entity<Sprint, string>
    {
        [MinLengthShouldBe(1)]
        [MaxLengthShouldBe(50)]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; protected set; } = string.Empty;

        public DateTime? StartDate { get; protected set; }

        public DateTime? EndDate { get; protected set; }

        public override ConditionProperty<Sprint>? Uniqueness()
        {
            return new ConditionProperty<Sprint>()
            {
                Condition = x => x.Name == Name,
                Description = Resource.ASprintWithThisNameOnThisProjectHasAlreadyExisted
            };
        }

        public static Sprint NewInstance()
        {
            return new Sprint();
        }

        public Sprint SetName(string value)
        {
            Name = value;

            return this;
        }

        public Sprint SetStartDate(DateTime? value)
        {
            StartDate = value;

            return this;
        }
        public Sprint SetEndDate(DateTime? value)
        {
            EndDate = value;

            return this;
        }

        public SprintViewModel ToViewModel(string projectName)
        {
            var viewModel = new SprintViewModel()
            {
                ModifiedDate = ModifiedDate,
                IsDeleted = IsDeleted,
                Id = Id!,
                Name = Name,
                ProjectName = projectName,
                StartDate = StartDate,
                EndDate = EndDate,
            };

            return viewModel;
        }
    }
}
