using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
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
                new FindSprint(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            base.Prepare(sprint);

            var parentProjectId = await mediator.Send(
                new FindProjectIdOfSprint(sprintId: sprint.Id, includeDeleted: true));

            InvariantState.AddAnInvariantRequest(
                new PreventIfTheSameSprintHasAlreadyExisted(
                    name: sprint.Name, parentProjectId: parentProjectId!, sprintId: sprint.Id));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, sprint);

            return sprint;
        }
    }

    public class RestoreSprintHandler :
    IRequestHandler<RestoreSprint>
    {
        private readonly IProjectSettingRepository _repository;
        public RestoreSprintHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            RestoreSprint request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
