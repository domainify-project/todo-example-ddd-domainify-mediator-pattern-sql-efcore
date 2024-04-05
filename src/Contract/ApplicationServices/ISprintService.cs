using Domain.ProjectSettingAggregation;
using Domainify.Domain;

namespace Contract
{
    public interface ISprintService
    {
        public Task<Guid> Process(DefineSprint request);
        public Task Process(ChangeSprintName request);
        public Task Process(ChangeSprintTimeSpan request);
        public Task Process(DeleteSprint request);
        public Task Process(CheckSprintForDeletingPermanently request);
        public Task Process(RestoreSprint request);
        public Task<SprintViewModel?> Process(GetSprint request);
        public Task<PaginatedList<SprintViewModel>> Process(GetSprintsList request);
    }
}
