using Contract;
using Domain.ProjectSetting;
using Domain.Task;
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

        [HttpGet($"/v1/projects/{{projectId}}/[controller]")]
        public async Task<ActionResult<PaginatedList<TaskViewModel>>> GetList(
            Guid projectId,
            Guid? sprintId = null,
            Domain.Task.TaskStatus? status = null,
            string? descriptionSearchKey = null)
        {
            var request = GetRequest<GetTasksList>();
            request.SetProjectId(projectId);

            return await _service.Process(request);
        }
        [HttpGet($"/v1.1/projects/{{projectId}}/[controller]")]
        public async Task<ActionResult<PaginatedList<TaskViewModel>>> GetList(
            Guid projectId,
            Guid? sprintId = null,
            Domain.Task.TaskStatus? status = null,
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
            var request = GetRequest<GetTask>();
            request.SetId(id);

            return await _service.Process(request);
        }

        [HttpPost]
        public async Task<ActionResult<TaskViewModel?>> Add(AddTask request)
        {
            var id = await _service.Process(request);
            return StatusCode(201, id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditTask request)
        {
            await _service.Process(request);
            return NoContent();
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeTaskStatus(ChangeTaskStatus request)
        {
            await _service.Process(request);
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = GetRequest<DeleteTask>();
            request.SetId(id);

            await _service.Process(request);
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            await _service.Process(new RestoreTask(id));
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanently(string id)
        {
            var request = GetRequest<DeleteTaskPermanently>();
            request.SetId(id);

            await _service.Process(request);
            return NoContent();
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<int, string>>>> GetTaskStatusItems()
        {
            var request = GetRequest<GetTaskStatusList>();
            return await _service.Process(request);
        }
    }
}