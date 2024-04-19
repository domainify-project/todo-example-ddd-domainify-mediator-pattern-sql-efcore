using MediatR;
using Domain.ProjectSetting;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ProjectSettingRepository : IProjectSettingRepository
    {
        private readonly TodoDbContext _dbContext;
        private readonly IMediator _mediator;
        public ProjectSettingRepository(
            TodoDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<string> Apply(DefineProject request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newItem = ProjectModel.InstanceOf(preparedEntity);
            _dbContext.Projects.Add(newItem);

            return newItem.Id.ToString();
        }

        public async Task Apply(ChangeProjectName request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Name = preparedEntity.Name;
            }
        }

        public async Task Apply(DeleteProject request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = true;
        }

        public async Task Apply(CheckProjectForDeleting request)
        {
            await request.ResolveAsync(_mediator);
        }

        public async Task Apply(RestoreProject request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = false;
        }

        public async Task Apply(DeleteProjectPermanently request)
        {
            await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Projects.Remove(itemToModify);
        }

        public async Task<ProjectViewModel?> Apply(GetProject request)
        {
            var query = _dbContext.Projects.AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithSprints)
                query = query.Include(i => i.Sprints);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var project = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, project!);

            return new ProjectViewModel(project!);
        }

        public async Task<PaginatedList<ProjectViewModel>> Apply(GetProjectsList request)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Projects.AsNoTracking().Where(i => i.IsDeleted == retrivalDeletationStatus);

            if (request.WithSprints)
                query = query.Include(i => i.Sprints);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.ToListAsync())
                .Select(i => new ProjectViewModel(i.ToEntity())).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<ProjectViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }

        public async Task<string> Apply(DefineSprint request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newItem = SprintModel.InstanceOf(preparedEntity, request.ProjectId);
            _dbContext.Sprints.Add(newItem);

            return newItem.Id.ToString();
        }

        public async Task Apply(ChangeSprintName request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Name = preparedEntity.Name;
            }
        }

        public async Task Apply(ChangeSprintTimeSpan request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.StartDate = preparedEntity.StartDate;
                itemToModify.EndDate = preparedEntity.EndDate;
            }
        }

        public async Task Apply(DeleteSprint request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = true;
        }

        public async Task Apply(DeleteSprintPermanently request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Sprints.Remove(itemToModify);
        }

        public async Task Apply(CheckSprintForDeleting request)
        {
            await request.ResolveAsync(_mediator);
        }

        public async Task Apply(RestoreSprint request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = false;
        }

        public async Task<SprintViewModel?> Apply(GetSprint request)
        {
            var query = _dbContext.Sprints.AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return new SprintViewModel(sprint!);
        }

        public async Task<PaginatedList<SprintViewModel>> Apply(GetSprintsList request)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Sprints
                .AsNoTracking().Where(i => i.IsDeleted == retrivalDeletationStatus);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.Select
                (i => new { Sprint = i, ProjectName = i.Project.Name }).ToListAsync())
                .Select(i => new SprintViewModel(i.Sprint.ToEntity())).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<SprintViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }

        public async Task<bool> Apply(PreventIfProjectHasSomeSprints request)
        {
            var result = await _dbContext.Projects
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id)
                && i.Sprints.Where(s => !s.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }

        public async Task<bool> Apply(PreventIfProjectHasSomeTasks request)
        {
            var result = await _dbContext.Projects
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id)
                && i.Tasks.Where(t => !t.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }

        public async Task<bool> Apply(PreventIfSprintHasSomeTasks request)
        {
            var result = await _dbContext.Sprints
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id)
                && i.Tasks.Where(t => !t.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }

        public async Task<bool> Apply(PreventIfTheSameProjectHasAlreadyExisted request)
        {
            await request.ResolveAsync(_mediator);

            var query = _dbContext.Projects
                .Where(i => !i.IsDeleted && i.Name == request.Name);

            if (!string.IsNullOrEmpty(request.ProjectId))
                query = query.Where(i => i.Id != new Guid(request.ProjectId));

            return await query.AnyAsync();
        }

        public async Task<bool> Apply(PreventIfTheSameSprintHasAlreadyExisted request)
        {
            await request.ResolveAsync(_mediator);

            var query = _dbContext.Sprints
                .Where(i => !i.IsDeleted && i.ProjectId == new Guid(request.ParentProjectId!)
                && i.Name == request.Name);

            if (!string.IsNullOrEmpty(request.SprintId))
                query = query.Where(i => i.Id != new Guid(request.SprintId));
            var res = await query.AnyAsync();

            return await query.AnyAsync();
        }

        public async Task<Project?> Apply(FindProject request)
        {
            var query = _dbContext.Projects
                  .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithSprints)
                query = query.Include(i => i.Sprints);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var project = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, project!);

            return project;
        }

        public async Task<string?> Apply(FindProjectIdOfSprint request)
        {
            var query = _dbContext.Sprints
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return retrievedItem?.ProjectId.ToString();
        }

        public async Task<Sprint?> Apply(FindSprint request)
        {
            var query = _dbContext.Sprints
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return sprint;
        }
    }
}
