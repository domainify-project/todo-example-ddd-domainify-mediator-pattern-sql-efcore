using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class GetSprint :
        QueryItemRequestById<Sprint, string, SprintViewModel?>
    {
        public bool WithTasks { get; private set; } = false;
        public GetSprint(string id, 
            bool withTasks = false, 
            bool includeDeleted = false) : base(id)
        {
            WithTasks = withTasks;
            PreventIfNoEntityWasFound = true;
            IncludeDeleted = includeDeleted;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Sprint sprint)
        {
            base.Prepare(sprint);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, sprint);
        }
    }

    public class GetSprintHandler :
         IRequestHandler<GetSprint, SprintViewModel?>
    {
        private readonly IProjectSettingRepository _repository;
        public GetSprintHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<SprintViewModel?> Handle(
            GetSprint request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
