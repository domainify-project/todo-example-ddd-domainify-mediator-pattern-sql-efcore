using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class RestoreSprint :
        RequestToRestoreById<Sprint, string>
    {
        public RestoreSprint(string id) 
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var sprint = (await mediator.Send(
                new FindSprint(Id, includeDeleted: true)))!;

            InvariantState.AddAnInvariantRequest(new PreventIfTheSameSprintHasAlreadyExisted(sprint));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }
}
