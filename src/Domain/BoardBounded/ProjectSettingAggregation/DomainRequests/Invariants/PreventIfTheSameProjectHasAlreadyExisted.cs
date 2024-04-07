using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfTheSameProjectHasAlreadyExisted
        : InvariantRequest<Project>
    {
        public Project Project { get; private set; }
        public PreventIfTheSameProjectHasAlreadyExisted(Project project)
        {
            Project = project;
        }
        public override IFault? GetFault()
        {
            return new AnEntityWithTheseUniquenessConditionsHasAlreadyExisted(
                    typeof(Project).Name, Description);
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
