using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    internal class SetTasksOfTheSprintToNoSprint :
        BulkCommandRequest<Task, List<string>>
    {
        public string SprintId { get; private set; } = string.Empty;

        public SetTasksOfTheSprintToNoSprint()
        {
            ValidationState.Validate();
        }

        public SetTasksOfTheSprintToNoSprint SetSprintId(string value)
        {
            SprintId = value;
            return this;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }

        public override async System.Threading.Tasks.Task ThenAsync(
            IMediator mediator, List<string> returnedItems)
        {
            foreach (string taskId in returnedItems)
                await mediator.Send(new DeleteTask(taskId));
        }
    }
}
