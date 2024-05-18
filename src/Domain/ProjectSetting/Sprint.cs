using Domainify.Domain;
using System.ComponentModel.DataAnnotations;

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

        public List<TaskAggregation.Task> Tasks { get; set; } = new List<TaskAggregation.Task>();


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
    }
}
