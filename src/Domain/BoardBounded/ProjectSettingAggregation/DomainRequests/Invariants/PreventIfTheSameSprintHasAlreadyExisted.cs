using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfTheSameSprintHasAlreadyExisted
        : InvariantRequest<Sprint>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string? ProjectId { get; private set; }
        public Sprint Sprint { get; private set; }
        public PreventIfTheSameSprintHasAlreadyExisted(Sprint sprint, string? projectId = null)
        {
            Sprint = sprint;
            ProjectId = projectId;
        }
        public override IIssue? GetIssue()
        {
            return new AnEntityWithTheseUniquenessConditionsHasAlreadyExisted(
                    typeof(Sprint).Name, Description);
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
