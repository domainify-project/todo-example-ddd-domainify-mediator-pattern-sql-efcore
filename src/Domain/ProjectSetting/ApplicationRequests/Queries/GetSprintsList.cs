using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSetting
{
    public class GetSprintsList :
        QueryListRequest<Sprint, PaginatedList<SprintViewModel>>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public required string ProjectId { get; set; }

        public bool? IsDeleted { get; set; }
        public string? SearchValue { get; set; } = string.Empty;
        public bool WithTasks { get; private set; } = false;
        public GetSprintsList(bool withTasks = false)
        {
            WithTasks = withTasks;
            ValidationState.Validate();
        }
        public GetSprintsList SetProjectId(string value)
        {
            ProjectId = value;
            return this;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class GetSprintsListHandler :
    IRequestHandler<GetSprintsList, PaginatedList<SprintViewModel>>
    {
        private readonly IProjectSettingRepository _repository;
        public GetSprintsListHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<SprintViewModel>> Handle(
            GetSprintsList request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
