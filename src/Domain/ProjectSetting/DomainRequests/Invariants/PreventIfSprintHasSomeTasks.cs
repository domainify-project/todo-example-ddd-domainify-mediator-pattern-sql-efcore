using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSetting
{
    public class PreventIfSprintHasSomeTasks
        : InvariantRequestById<Sprint, string>
    {
        public PreventIfSprintHasSomeTasks(string id) : base(id)
        {
        }

        public override IFault? GetFault()
        {
            return new SomeTasksHaveBeenDefinedForThisSprintFault();
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class PreventIfSprintHasSomeTasksHandler :
        IRequestHandler<PreventIfSprintHasSomeTasks, bool>
    {
        private readonly IProjectSettingRepository _repository;
        public PreventIfSprintHasSomeTasksHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            PreventIfSprintHasSomeTasks request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
