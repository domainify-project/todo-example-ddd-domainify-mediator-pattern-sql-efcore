using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindProjectIdOfSprint :
        QueryItemRequestById<Sprint, string, string?>
    {
        public FindProjectIdOfSprint(string sprintId,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(sprintId)
        {
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async Task ResolveAsync(IMediator mediator, Sprint sprint)
        {
            base.Prepare(sprint);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, sprint);
        }
    }
}
