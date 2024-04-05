using Domain.ProjectSettingAggregation;
using Domainify.Domain;

namespace Contract
{
    public interface IProjectService
    {
        public Task<string> Process(DefineProject request);
        public Task Process(ChangeProjectName request);
        public Task Process(DeleteProject request);
        public Task Process(CheckProjectForDeletingPermanently request);
        public Task Process(RestoreProject request);
        public Task Process(DeleteProjectPermanently request);
        public Task<ProjectViewModel?> Process(GetProject request);
        public Task<PaginatedList<ProjectViewModel>> Process(GetProjectsList request);
    }
}
