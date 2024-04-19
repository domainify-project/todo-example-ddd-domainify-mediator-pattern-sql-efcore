using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class FindSprint :
        QueryItemRequestById<Sprint, string, Sprint?>
    {
        public bool WithTasks { get; private set; } = false;
        public FindSprint(string id,
            bool withTasks = false,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(id)
        {
            WithTasks = withTasks;
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

    public class FindSprintHandler :
        IRequestHandler<FindSprint, Sprint?>
    {
        private readonly IProjectSettingRepository _repository;
        public FindSprintHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sprint?> Handle(
            FindSprint request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
