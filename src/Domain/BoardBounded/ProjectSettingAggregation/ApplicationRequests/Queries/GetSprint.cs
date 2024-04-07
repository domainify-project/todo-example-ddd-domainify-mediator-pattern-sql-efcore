using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class GetSprint :
        QueryItemRequestById<Sprint, string, SprintViewModel?>
    {
        public GetSprint(string id, bool includeDeleted = false) : base(id)
        {
            PreventIfNoEntityWasFound = true;
            IncludeDeleted = includeDeleted;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
