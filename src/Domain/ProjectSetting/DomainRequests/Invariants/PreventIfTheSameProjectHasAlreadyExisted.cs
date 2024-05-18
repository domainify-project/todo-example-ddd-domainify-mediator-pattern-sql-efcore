using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfTheSameProjectHasAlreadyExisted
        : InvariantRequest<Project>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string? ProjectId { get; private set; }

        [BindTo(typeof(Project), nameof(Project.Name))]
        public string Name { get; protected set; }
        public PreventIfTheSameProjectHasAlreadyExisted(string name, string? projectId = null)
        {
            Name = name;
            ProjectId = projectId;
        }
        public override IFault? GetFault()
        {
            return new AnEntityWithThesePropertiesHasAlreadyExistedFault(
                    typeof(Project).Name, Description);
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
