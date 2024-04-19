using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class FindProjectIdOfSprint :
        QueryItemRequestById<Sprint, string, string?>
    {
        public FindProjectIdOfSprint(string sprintId,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(sprintId)
        {
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Sprint sprint)
        {
            base.Prepare(sprint);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, sprint);
        }
    }

    public class FindProjectIdOfSprintHandler :
    IRequestHandler<FindProjectIdOfSprint, string?>
    {
        private readonly IProjectSettingRepository _repository;
        public FindProjectIdOfSprintHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<string?> Handle(
            FindProjectIdOfSprint request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
