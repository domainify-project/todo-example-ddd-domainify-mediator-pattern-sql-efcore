using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindProject :
        QueryItemRequestById<Project, string, Project?>
    {
        public bool WithSprints { get; private set; } = false;
        public FindProject(string id, bool withSprints = false, bool includeDeleted = false) : base(id)
        {
            WithSprints = withSprints;
            IncludeDeleted = includeDeleted;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
