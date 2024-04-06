using Contract;
using Domain.TaskAggregation;
using Domainify.AspMvc;
using Domainify.Domain;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.WebAPI
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TasksController : ApiController
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet($"/v1/{nameof(ProjectSettingController)}/{{{nameof(GetTasksList.ProjectId)}}}/[controller]")]
        public async Task<ActionResult<PaginatedList<TaskViewModel>>> GetList(
            Guid projectId,
            Guid? sprintId = null,
            Domain.TaskAggregation.TaskStatus? status = null,
            string? descriptionSearchKey = null)
        {
            var request = GetRequest<GetTasksList>();
            request.SetProjectId(projectId);
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _service.Process(request);
        }
        [HttpGet($"/v1.1/{nameof(ProjectSettingController)}/{{{nameof(GetTasksList.ProjectId)}}}/[controller]")]
        public async Task<ActionResult<PaginatedList<TaskViewModel>>> GetList(
            Guid projectId,
            Guid? sprintId = null,
            Domain.TaskAggregation.TaskStatus? status = null,
            string? descriptionSearchKey = null,
            int? pageNumber = null,
            int? pageSize = null)
        {
            var request = GetRequest<GetTasksList>();
            request.SetProjectId(projectId);
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _service.Process(request);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel?>> Get(string id)
        {
            return await View(
                () => _service.Process(new GetTask(id)));
        }

        [HttpPost]
        public async Task<ActionResult<TaskViewModel?>> Add(AddTask request)
        {
            var id = await _service.Process(request);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditTask request)
        {
            return await View(
                () => _service.Process(request));
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeTaskStatus(ChangeTaskStatus request)
        {
            return await View(
                () => _service.Process(request));
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await View(
                () => _service.Process(new DeleteTask(id)));
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            return await View(
                () => _service.Process(new RestoreTask(id)));
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<int, string>>>> GetTaskStatusItems()
        {
            var request = GetRequest<GetTaskStatusList>();
            return await _service.Process(request);
        }
    }
}