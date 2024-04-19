using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSetting
{
    public class PreventIfProjectHasSomeTasks
        : InvariantRequestById<Project, string>
    {
        public PreventIfProjectHasSomeTasks(string id) : base(id)
        {
        }

        public override IFault? GetFault()
        {
            return new SomeTasksHaveBeenDefinedForThisProjectFault();
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class PreventIfProjectHasSomeTasksHandler :
    IRequestHandler<PreventIfProjectHasSomeTasks, bool>
    {
        private readonly IProjectSettingRepository _repository;
        public PreventIfProjectHasSomeTasksHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            PreventIfProjectHasSomeTasks request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
