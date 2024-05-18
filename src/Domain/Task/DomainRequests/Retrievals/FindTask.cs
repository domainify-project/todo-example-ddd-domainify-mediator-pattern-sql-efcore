using MediatR;
using Domainify.Domain;

namespace Domain.TaskAggregation
{
    internal class FindTask :
        QueryItemRequestById<Task, string, Task?>
    {
        public FindTask(string id,
            bool includeDeleted = false, bool preventIfNoEntityWasFound = false)
            : base(id)
        {
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Task task)
        {
            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);
        }
    }
}
