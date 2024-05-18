using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckSprintForDeleting :
        RequestToCheckEntityForDeletingById<Sprint, string>
    {
        public CheckSprintForDeleting(string id)
            : base(id)
        {
        }

        public override async Task ResolveAsync(IMediator mediator)
        {
            var sprint = (await mediator.Send(
                new FindSprint(Id,
                preventIfNoEntityWasFound: true,
                includeDeleted: true)))!;

            base.Prepare(sprint);

            InvariantState.AddAnInvariantRequest(new PreventIfSprintHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, sprint);
        }
    }
}
