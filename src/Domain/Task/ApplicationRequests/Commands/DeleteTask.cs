using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    public class DeleteTask :
        RequestToDeleteById<Task, string>
    {
        public DeleteTask(string id)
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true)))!;

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);

            return task;
        }
    }
}