using Domain.ProjectSettingAggregation;
using Domainify.Domain;

namespace Contract
{
    public interface IProjectSettingService
    {
        public Task<string> Process(DefineProject request);
        public Task Process(ChangeProjectName request);
        public Task Process(DeleteProject request);
        public Task Process(CheckProjectForDeletingPermanently request);
        public Task Process(RestoreProject request);
        public Task Process(DeleteProjectPermanently request);
        public Task<ProjectViewModel?> Process(GetProject request);
        public Task<PaginatedList<ProjectViewModel>> Process(GetProjectsList request);
        public Task<string> Process(DefineSprint request);
        public Task Process(ChangeSprintName request);
        public Task Process(ChangeSprintTimeSpan request);
        public Task Process(DeleteSprint request);
        public Task Process(CheckSprintForDeletingPermanently request);
        public Task Process(RestoreSprint request);
        public Task<SprintViewModel?> Process(GetSprint request);
        public Task<PaginatedList<SprintViewModel>> Process(GetSprintsList request);
    }
}
