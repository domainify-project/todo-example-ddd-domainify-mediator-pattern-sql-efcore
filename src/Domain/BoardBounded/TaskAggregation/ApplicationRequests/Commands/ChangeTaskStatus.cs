using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    public class ChangeTaskStatus :
        RequestToUpdateById<Task, string>
    {
        public TaskStatus Status { get; private set; }

        public ChangeTaskStatus(string id, TaskStatus status) 
            : base(id)
        {
            Status = status;

            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var task = (await mediator.Send(new FindTask(Id)))!;
            task.SetStatus(Status);
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }
}
