using Contract;
using Domain.ProjectSettingAggregation;
using Domainify.AspMvc;
using Domainify.Domain;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.WebAPI
{
    [ApiController]
    [Route("v1/[controller]")]
    public class SprintsController : ApiController
    {
        private readonly IProjectSettingService _service;

        public SprintsController(IProjectSettingService service)
        {
            _service = service;
        }
        [HttpGet($"/v1/Projects/{{projectId}}/[controller]")]
        public async Task<ActionResult<PaginatedList<SprintViewModel>>> GetList(
            string projectId)
        {
            var request = GetRequest<GetSprintsList>()
                           .SetProjectId(projectId);
            return await _service.Process(request);
        }
        [HttpGet($"/v1.1/Projects/{{projectId}}/[controller]")]
        public async Task<ActionResult<PaginatedList<SprintViewModel>>> GetList(
            string projectId, 
            int? pageNumber = null,
            int? pageSize = null, 
            bool? withTasks = null)
        {
            var request = GetRequest<GetSprintsList>()
                           .SetProjectId(projectId);
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _service.Process(request);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SprintViewModel?>> Get(string id, bool? withTasks = null)
        {
            var request = GetRequest<GetSprint>();
            request.SetId(id);

            return await _service.Process(request);
        }

        [HttpPost]
        public async Task<ActionResult<SprintViewModel?>> Define(DefineSprint request)
        {
            var id = await _service.Process(request);
            return StatusCode(201, id);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeSprintName(ChangeSprintName request)
        {
            await _service.Process(request);
            return NoContent();
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeSprintTimeSpan(ChangeSprintTimeSpan request)
        {
            await _service.Process(request);
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(
            string id, bool deleteAllRelatedTask)
        {
            var request = GetRequest<DeleteSprint>();
            request.SetId(id);

            await _service.Process(request);
            return NoContent();
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> CheckSprintForDeletingPermanently(string id)
        {
            await _service.Process(new CheckSprintForDeleting(id));
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            await _service.Process(new RestoreSprint(id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanently(
            string id, bool deleteAllRelatedTask)
        {
            var request = GetRequest<DeleteSprintPermanently>();
            request.SetId(id);

            await _service.Process(request);
            return NoContent();
        }
    }
}