using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class ChangeProjectName :
        RequestToUpdateById<Project, string>
    {
        [BindTo(typeof(Project), nameof(Project.Name))]
        public string Name { get; private set; }
 
        public ChangeProjectName(string id, string name) 
            : base(id)
        {
            Name = name.Trim();
            ValidationState.Validate();
        }

        public override async Task<Project> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var project = (await mediator.Send(new FindProject(Id, preventIfNoEntityWasFound: true)))!;
            project.SetName(Name);

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfTheSameProjectHasAlreadyExisted(name: project.Name, projectId: project.Id));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, project);
            return project;
        }
    }
}
