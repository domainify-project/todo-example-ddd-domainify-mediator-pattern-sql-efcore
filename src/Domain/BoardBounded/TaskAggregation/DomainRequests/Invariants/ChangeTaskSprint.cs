using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    internal class ChangeTaskSprint :
        RequestToUpdateById<Task, string>
    {
        public string? SprintId { get; private set; }

        public ChangeTaskSprint(string id, string? sprintId = null) 
            : base(id)
        {
            SprintId = sprintId;

            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var task = await mediator.Send(new FindTask(Id));
            task!.SetSprintId(SprintId);
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }
}
