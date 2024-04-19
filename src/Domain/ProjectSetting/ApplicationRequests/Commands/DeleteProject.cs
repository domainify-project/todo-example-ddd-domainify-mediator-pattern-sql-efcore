using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class DeleteProject :
        RequestToDeleteById<Project, string>
    {
        public DeleteProject(string id) 
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Project> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeSprints(id: Id));
            InvariantState.AddAnInvariantRequest(new PreventIfProjectHasSomeTasks(id: Id));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
            return project;
        }
    }

    public class DeleteProjectHandler :
    IRequestHandler<DeleteProject>
    {
        private readonly IProjectSettingRepository _repository;
        public DeleteProjectHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteProject request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}