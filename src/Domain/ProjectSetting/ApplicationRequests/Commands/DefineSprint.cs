using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DefineSprint
        : RequestToCreate<Sprint, string>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string ProjectId { get; private set; } = string.Empty;

        [BindTo(typeof(Sprint), nameof(Sprint.Name))]
        public string Name { get; private set; }

        public DefineSprint(string projectId, string name)
        {
            ProjectId = projectId.Trim();
            Name = name.Trim();
            ValidationState.Validate();
        }

        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var sprint = Sprint.NewInstance()
                .SetName(Name);
   
            InvariantState.AddAnInvariantRequest(
                new PreventIfTheSameSprintHasAlreadyExisted(name: sprint.Name, parentProjectId: ProjectId));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }
}
