using Microsoft.AspNetCore.Mvc;
using Domainify.AspMvc;
using Contract;
using Domainify.Domain;
using Domain.ProjectSettingAggregation;

namespace Presentation.WebAPI
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProjectSettingController : ApiController
    {
        private readonly IProjectSettingService _projectSettingService;
        private readonly ITaskService _taskService;
        public ProjectSettingController(
            IProjectSettingService projectService,
            ITaskService taskService)
        {
            _projectSettingService = projectService;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProjectViewModel>>> GetList()
        {
            var request = GetRequest<GetProjectsList>();
            return await _projectSettingService.Process(request);
        }

        /// <summary>
        /// You can send a querystring like the following example,
        /// for adjusting the offset and limit values
        /// Example: offset=0&limit=5
        /// </summary>
        /// <returns></returns>
        [HttpGet($"/v1.1/[controller]")]
        public async Task<ActionResult<PaginatedList<ProjectViewModel>>> GetList(
            int? pageNumber = null,
            int? pageSize = null)
        {
            var request = GetRequest<GetProjectsList>();
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _projectSettingService.Process(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModel?>> Get(string id)
        {
            return await View(() => _projectSettingService.Process(new GetProject(id)));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectViewModel?>> Define(DefineProject request)
        {
            var id = await _projectSettingService.Process(request);

            return StatusCode(201, id);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeName(ChangeProjectName request)
        {
            await _projectSettingService.Process(request);
            return Ok();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await View(
                () => _projectSettingService.Process(new DeleteProject(id)));
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> CheckProjectForDeletingPermanently(string id)
        {
            return await View(
                () => _projectSettingService.Process(new CheckProjectForDeletingPermanently(id)));
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            return await View(
                () => _projectSettingService.Process(new RestoreProject(id)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanently(string id)
        {
            return await View(
                () => _projectSettingService.Process(new DeleteProjectPermanently(id)));
        }
    }
}