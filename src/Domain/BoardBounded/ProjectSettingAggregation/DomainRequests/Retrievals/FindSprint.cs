using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindSprint :
        QueryItemRequestById<Sprint, string, Sprint?>
    {
        public bool WithTasks { get; private set; } = false;
        public FindSprint(string id,
            bool withTasks = false,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(id)
        {
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
