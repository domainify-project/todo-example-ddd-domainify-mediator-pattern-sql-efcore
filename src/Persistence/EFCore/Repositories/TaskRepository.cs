using MediatR;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;
using Domain.Task;

namespace Persistence
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoDbContext _dbContext;
        private readonly IMediator _mediator;
        public TaskRepository(
            TodoDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<string> Apply(AddTask request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newItem = TaskModel.InstanceOf(
                preparedEntity,
                projectId: request.ProjectId,
                sprintId: request.SprintId);

            _dbContext.Tasks.Add(newItem);

            return newItem.Id.ToString();
        }

        public async System.Threading.Tasks.Task Apply(EditTask request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Description = preparedEntity.Description;
                itemToModify.Status = preparedEntity.Status;
                itemToModify.SprintId = request.SprintId == null ? null : Guid.Parse(request.SprintId);
            }
        }

        public async System.Threading.Tasks.Task Apply(DeleteTask request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = true;
        }

        public async System.Threading.Tasks.Task Apply(DeleteTaskPermanently request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Tasks.Remove(itemToModify);
        }

        public async System.Threading.Tasks.Task Apply(ChangeTaskStatus request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Status = preparedEntity.Status;
            }
        }

        public async System.Threading.Tasks.Task Apply(RestoreTask request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = false;
        }

        public async Task<PaginatedList<TaskViewModel>> Apply(GetTasksList request)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Tasks
                .Include(i => i.Project)
                .Include(i => i.Sprint!)
                .AsNoTracking()
                .Where(i => i.IsDeleted == retrivalDeletationStatus);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.ToListAsync())
                .Select(i =>
                new TaskViewModel(i.ToEntity(),
                projectName: i.Project.Name,
                sprintName: i.Sprint!.Name)).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<TaskViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }

        public async System.Threading.Tasks.Task Apply(ChangeTaskSprint request)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.SprintId = new Guid(request.SprintId!);
            }
        }

        public async System.Threading.Tasks.Task Apply(DeleteAllRelatedTasksOfSprint request)
        {
            await request.ResolveAsync(_mediator);
        }

        public async Task<TaskViewModel?> Apply(GetTask request)
        {
            var query = _dbContext.Tasks
            .Include(i => i.Project)
            .Include(i => i.Sprint!)
            .AsNoTracking()
            .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var task = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, task!);

            return new TaskViewModel(task!,
                projectName: retrievedItem!.Project.Name,
                sprintName: retrievedItem.Sprint?.Name);
        }

        public async Task<Domain.Task.Task?> Apply(FindTask request)
        {
            var query = _dbContext.Tasks
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);


            var retrievedItem = await query.FirstOrDefaultAsync();
            var task = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, task!);

            return task;
        }

        public async Task<List<string>> Apply(RetrieveTasksIdsListOfTheSprint request)
        {
            var tasksIdsList = await _dbContext.Tasks
            .Where(i => i.SprintId == new Guid(request.SprintId))
            .Select(i => i.Id.ToString()).ToListAsync();

            return tasksIdsList;
        }
    }
}
