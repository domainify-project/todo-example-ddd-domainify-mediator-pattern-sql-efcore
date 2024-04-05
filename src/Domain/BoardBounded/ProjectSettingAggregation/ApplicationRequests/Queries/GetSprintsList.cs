using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    public class GetSprintsList :
        QueryListRequest<Sprint, List<SprintViewModel>>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public required string ProjectId { get; set; }

        public bool? IsDeleted { get; set; }
        public string? SearchValue { get; set; } = string.Empty;

        public GetSprintsList()
        {
            ValidationState.Validate();
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
