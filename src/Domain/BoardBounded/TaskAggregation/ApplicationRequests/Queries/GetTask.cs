using MediatR;
using Domainify.Domain;

namespace Domain.TaskAggregation
{
    public class GetTask :
        QueryItemRequestById<Task, string, Task?>
    {
        public GetTask(string id) : base(id)
        {
            PreventIfNoEntityWasFound = true;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
