using Domainify.Domain;
using ThreadTask = System.Threading.Tasks.Task;

namespace Domain.ProjectSetting
{
    public interface IProjectSettingRepository
    {
        #region application requests
        public Task<string> Apply(DefineProject request);
        public ThreadTask Apply(ChangeProjectName request);
        public ThreadTask Apply(DeleteProject request);
        public ThreadTask Apply(CheckProjectForDeleting request);
        public ThreadTask Apply(RestoreProject request);
        public ThreadTask Apply(DeleteProjectPermanently request);
        public Task<ProjectViewModel?> Apply(GetProject request);
        public Task<PaginatedList<ProjectViewModel>> Apply(GetProjectsList request);
        public Task<string> Apply(DefineSprint request);
        public ThreadTask Apply(ChangeSprintName request);
        public ThreadTask Apply(ChangeSprintTimeSpan request);
        public ThreadTask Apply(DeleteSprint request);
        public ThreadTask Apply(DeleteSprintPermanently request);
        public ThreadTask Apply(CheckSprintForDeleting request);
        public ThreadTask Apply(RestoreSprint request);
        public Task<SprintViewModel?> Apply(GetSprint request);
        public Task<PaginatedList<SprintViewModel>> Apply(GetSprintsList request);

        #endregion

        #region domain requests
        public Task<bool> Apply(PreventIfProjectHasSomeSprints request);
        public Task<bool> Apply(PreventIfProjectHasSomeTasks request);
        public Task<bool> Apply(PreventIfSprintHasSomeTasks request);
        public Task<bool> Apply(PreventIfTheSameProjectHasAlreadyExisted request);
        public Task<bool> Apply(PreventIfTheSameSprintHasAlreadyExisted request);
        public Task<Project?> Apply(FindProject request);
        public Task<string?> Apply(FindProjectIdOfSprint request);
        public Task<Sprint?> Apply(FindSprint request);
        #endregion
    }
}
