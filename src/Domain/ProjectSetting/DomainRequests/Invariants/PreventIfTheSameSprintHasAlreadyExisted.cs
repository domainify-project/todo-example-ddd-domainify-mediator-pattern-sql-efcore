using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfTheSameSprintHasAlreadyExisted
        : InvariantRequest<Sprint>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string ParentProjectId { get; private set; }

        [BindTo(typeof(Sprint), nameof(Sprint.Id))]
        public string? SprintId { get; private set; }

        [BindTo(typeof(Sprint), nameof(Sprint.Name))]
        public string Name { get; protected set; }

        public PreventIfTheSameSprintHasAlreadyExisted(
            string name,
            string parentProjectId,
            string? sprintId = null)
        {
            Name = name;
            ParentProjectId = parentProjectId;
            SprintId = sprintId;
        }
        public override IFault? GetFault()
        {
            return new AnEntityWithThesePropertiesHasAlreadyExistedFault(
                    typeof(Sprint).Name, Description);
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
