using Domainify.Domain;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Domain.TaskAggregation
{
    public class GetTasksList :
        QueryListRequest<Task, PaginatedList<TaskViewModel>>
    {
        public bool? IsDeleted { get; set; }

        [Required]
        public Guid ProjectId { get; private set; }
        public Guid? SprintId { get; set; }
        public TaskStatus? Status { get; set; }
        public string? DescriptionSearchKey { get; set; }

        public GetTasksList()
        {
            ValidationState.Validate();
        }
        public GetTasksList SetProjectId(Guid value)
        {
            ProjectId = value;
            return this;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
