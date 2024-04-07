using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DeleteProject :
        RequestToDeleteById<Project, string>
    {
        public DeleteProject(string id) 
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Project> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
            return project;
        }
    }
}