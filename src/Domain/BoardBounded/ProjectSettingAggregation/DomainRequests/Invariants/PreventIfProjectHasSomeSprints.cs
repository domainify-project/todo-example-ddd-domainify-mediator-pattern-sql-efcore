using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    internal class PreventIfProjectHasSomeSprints
        : InvariantRequestById<Project, string>
    {
        public PreventIfProjectHasSomeSprints(string id) : base(id)
        {
        }

        public override IFault? GetFault()
        {
            return new TheProjectHasSomeSprintsFault();
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
