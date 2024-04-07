using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindSprint :
        QueryItemRequestById<Sprint, string, Sprint?>
    {
        public FindSprint(string id,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(id)
        {
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
