using Domainify.Domain;
using MediatR;

namespace Domain.TaskAggregation
{
    public class DeleteTaskPermanently :
        RequestToDeletePermanentlyById<Task, string>
    {
        public DeleteTaskPermanently(string id)
            : base(id)
        { 
            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(IMediator mediator)
        {
            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);

            return task;
        }
    }
}
