using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    internal class FindSprint :
        QueryItemRequestById<Sprint, string, Sprint?>
    {
        public FindSprint(string id, bool includeDeleted =   false) : base(id)
        {
            IncludeDeleted = includeDeleted;
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
