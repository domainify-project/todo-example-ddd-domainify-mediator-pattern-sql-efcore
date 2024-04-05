using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfSprintHasSomeTasks
        : InvariantRequestById<Sprint, string>
    {
        public PreventIfSprintHasSomeTasks(string id) : base(id)
        {
        }

        public override IIssue? GetIssue()
        {
            return new SomeTasksHaveBeenDefinedForThisSprint();
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
