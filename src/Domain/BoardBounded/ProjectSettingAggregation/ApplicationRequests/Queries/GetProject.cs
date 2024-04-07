using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class GetProject :
        QueryItemRequestById<Project, string, Project?>
    {
        public bool WithSprints { get; private set; } = false;
        public GetProject(string id, bool withSprints = false, bool includeDeleted = false) : base(id)
        {
            WithSprints = withSprints;
            TrackingMode = true;
            PreventIfNoEntityWasFound = true;
            IncludeDeleted = includeDeleted;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
