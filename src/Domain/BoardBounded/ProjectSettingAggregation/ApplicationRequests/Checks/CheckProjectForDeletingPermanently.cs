using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckProjectForDeletingPermanently :
        RequestToCheckEntityForDeletingById<Project, string>
    {
        public CheckProjectForDeletingPermanently(string id)
            : base(id)
        {
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id, preventIfNoEntityWasFound: true)))!;
            await base.ResolveAsync(mediator, project);

            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);
        }


    }
}
