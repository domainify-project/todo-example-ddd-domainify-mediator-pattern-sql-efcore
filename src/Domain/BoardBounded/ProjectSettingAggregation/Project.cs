using Domainify.Domain;
using System.ComponentModel.DataAnnotations;
using Domain.Properties;

namespace Domain.ProjectSettingAggregation
{
    public class Project : Entity<Project, string>, IAggregateRoot
    {
        public double Version { get; set; }

        [MinLengthShouldBe(1)]
        [MaxLengthShouldBe(50)]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; protected set; } = string.Empty;

        public List<Sprint> Sprints { get; set; } = new List<Sprint>();
        public List<TaskAggregation.Task> Tasks { get; set; } = new List<TaskAggregation.Task>();

        public Project ()
        {
            // The version field is used for persisting in a nosql database like MongoDB
            //Version = 1.0;
        }

        public static Project NewInstance()
        {
            return new Project();
        }

        public Project SetName(string value)
        {
            Name = value;

            return this;
        }
    }
}
