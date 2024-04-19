using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class CheckSprintForDeleting :
        RequestToCheckEntityForDeletingById<Sprint, string>
    {
        public CheckSprintForDeleting(string id)
            : base(id)
        {
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
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

    public class CheckProjectForDeletingPermanentlyHandler :
    IRequestHandler<CheckProjectForDeleting>
    {
        private readonly IProjectSettingRepository _repository;
        public CheckProjectForDeletingPermanentlyHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            CheckProjectForDeleting request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
