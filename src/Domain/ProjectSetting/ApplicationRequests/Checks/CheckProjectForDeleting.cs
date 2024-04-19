using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class CheckProjectForDeleting :
        RequestToCheckEntityForDeletingById<Project, string>
    {
        public CheckProjectForDeleting(string id)
            : base(id)
        {
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id
                , preventIfNoEntityWasFound: true
                , includeDeleted: true)))!;

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
        }
    }

    public class CheckProjectForDeletingHandler :
        IRequestHandler<CheckProjectForDeleting>
    {
        private readonly IProjectSettingRepository _repository;
        public CheckProjectForDeletingHandler(IProjectSettingRepository repository)
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
