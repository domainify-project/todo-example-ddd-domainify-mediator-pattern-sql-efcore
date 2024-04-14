using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckProjectForDeleting :
        RequestToCheckEntityForDeletingById<Project, string>
    {
        public CheckProjectForDeleting(string id)
            : base(id)
        {
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id
                , preventIfNoEntityWasFound: true
                , includeDeleted: true)))!;

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
        }
    }
}
