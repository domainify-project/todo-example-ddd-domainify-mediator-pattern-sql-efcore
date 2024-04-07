using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckSprintForDeletingPermanently :
        RequestToCheckEntityForDeletingById<Sprint, string>
    {
        public CheckSprintForDeletingPermanently(string id)
            : base(id)
        {
        }

        public override async Task ResolveAsync(IMediator mediator)
        {
            var sprint = (await mediator.Send(
                new FindSprint(Id, preventIfNoEntityWasFound: true)))!;
            await base.ResolveAsync(mediator, sprint);

            InvariantState.AddAnInvariantRequest(new PreventIfSprintHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);
        }
    }
}
