using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class CheckSprintForDeletingPermanently :
        AnyRequestById<Sprint, string>
    {
        public CheckSprintForDeletingPermanently(string id)
            : base(id)
        {
        }

        public override async Task ResolveAsync(IMediator mediator)
        {
            InvariantState.AddAnInvariantRequest(new PreventIfSprintHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);
        }
    }
}
