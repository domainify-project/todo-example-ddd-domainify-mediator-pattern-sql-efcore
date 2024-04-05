using MediatR;
using Domainify.Domain;

namespace Domain.TaskAggregation
{
    internal class RetrieveTasksIdsListOfTheSprint :
        QueryListRequest<Task, List<string>>
    {
        public string SprintId { get; private set; } = string.Empty;

        public RetrieveTasksIdsListOfTheSprint()
        {
            ValidationState.Validate();
        }

        public RetrieveTasksIdsListOfTheSprint SetSprintId(string value)
        {
            SprintId = value;
            return this;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
