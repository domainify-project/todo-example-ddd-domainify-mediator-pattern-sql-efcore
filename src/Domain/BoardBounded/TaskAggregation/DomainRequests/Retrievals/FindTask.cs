using MediatR;
using Domainify.Domain;

namespace Domain.TaskAggregation
{
    internal class FindTask :
        QueryItemRequestById<Task, string, Task?>
    {
        public FindTask(string id,
            bool includeDeleted = false)
            : base(id)
        {
            IncludeDeleted = includeDeleted;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
