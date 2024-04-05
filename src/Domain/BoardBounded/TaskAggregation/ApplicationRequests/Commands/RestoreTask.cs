using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    public class RestoreTask :
        RequestToRestoreById<Task, string>
    {
        public RestoreTask(string id)
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true)))!;
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }
}
