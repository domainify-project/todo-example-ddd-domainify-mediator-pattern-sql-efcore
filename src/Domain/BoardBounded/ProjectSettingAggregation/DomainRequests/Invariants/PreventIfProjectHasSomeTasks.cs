using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfProjectHasSomeTasks
        : InvariantRequestById<Project, string>
    {
        public PreventIfProjectHasSomeTasks(string id) : base(id)
        {
        }

        public override IFault? GetFault()
        {
            return new SomeTasksHaveBeenDefinedForThisProject();
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
