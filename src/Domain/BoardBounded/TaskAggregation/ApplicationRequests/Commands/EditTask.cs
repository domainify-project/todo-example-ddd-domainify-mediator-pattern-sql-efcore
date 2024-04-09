using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    public class EditTask :
        RequestToUpdateById<Task, string>
    {
        [BindTo(typeof(Task), nameof(Task.Description))]
        public string Description { get; private set; }
        public string? SprintId { get; private set; }
        public TaskStatus Status { get; private set; }

        public EditTask(
            string id,  string description, TaskStatus status, string? sprintId = null)
            : base(id)
        {
            Description = description.Trim();
            Status = status;
            SprintId = sprintId;

            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var task = (await mediator.Send(new FindTask(Id)))!;
            task.SetDescription(Description)
                .SetStatus(Status);
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }
}
