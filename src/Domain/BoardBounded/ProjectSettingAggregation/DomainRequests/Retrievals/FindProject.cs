using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindProject :
        QueryItemRequestById<Project, string, Project?>
    {
        public bool WithSprints { get; private set; } = false;
        public bool WithTasks { get; private set; } = false;

        public FindProject(string id,
            bool withSprints = false,
            bool withTasks = false,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(id)
        {
            WithSprints = withSprints;
            WithTasks = withTasks;
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
