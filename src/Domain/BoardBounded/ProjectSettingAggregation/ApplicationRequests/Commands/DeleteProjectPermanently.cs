using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DeleteProjectPermanently :
        RequestToDeletePermanentlyById<Project, string>
    {
        public DeleteProjectPermanently(string id)
            : base(id)
        { 
            ValidationState.Validate();
        }

        public override async Task<Project> ResolveAndGetEntityAsync(IMediator mediator)
        {
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            var project = (await mediator.Send(
                new FindProject(Id, includeDeleted: true)))!;
            await base.ResolveAsync(mediator, project);
            return project;
        }
    }
}
