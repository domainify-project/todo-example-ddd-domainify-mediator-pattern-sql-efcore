using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
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
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
