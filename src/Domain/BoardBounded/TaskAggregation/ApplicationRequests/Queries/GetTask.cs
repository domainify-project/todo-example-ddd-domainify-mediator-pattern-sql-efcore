using MediatR;
using Domainify.Domain;
using Domain.ProjectSettingAggregation;

namespace Domain.TaskAggregation
{
    public class GetTask :
        QueryItemRequestById<Task, string, Task?>
    {
        public GetTask(string id) : base(id)
        {
            PreventIfNoEntityWasFound = true;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Task task)
        {
            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);
        }
    }
}
