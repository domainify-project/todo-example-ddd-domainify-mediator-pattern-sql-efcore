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
            var project = (await mediator.Send(new FindProject(Id)))!;
            project.SetName(Name);

            InvariantState.AddAnInvariantRequest(new PreventIfTheSameProjectHasAlreadyExisted(project));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, project);
            return project;
        }
    }
}
