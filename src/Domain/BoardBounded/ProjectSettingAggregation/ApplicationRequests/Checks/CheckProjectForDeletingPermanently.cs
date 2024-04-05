using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckProjectForDeletingPermanently :
        AnyRequestById<Project, string>
    {
        public CheckProjectForDeletingPermanently(string id)
            : base(id)
        {
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);
        }
    }
}
