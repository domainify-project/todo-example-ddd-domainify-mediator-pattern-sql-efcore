using Microsoft.AspNetCore.Mvc;
using Domainify.AspMvc;
using Contract;
using Domainify.Domain;
using Domain.ProjectSettingAggregation;

namespace Presentation.WebAPI
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProjectsController : ApiController
    {
        private readonly IProjectSettingService _projectSettingService;

        public ProjectsController(
            IProjectSettingService projectService)
        {
            _projectSettingService = projectService;
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
            int? pageSize = null,
            bool? withSprints = null,
            bool? withTasks = null)
        {
            var request = GetRequest<GetProjectsList>();
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _projectSettingService.Process(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModel?>> Get(
            string id, bool? withSprints = null, bool? withTasks = null)
        {
            var request = GetRequest<GetProject>();
            request.SetId(id);

            return await _projectSettingService.Process(request);
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
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _projectSettingService.Process(new DeleteProject(id));
            return NoContent();
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> CheckProjectForDeletingPermanently(string id)
        {
            await _projectSettingService.Process(new CheckProjectForDeletingPermanently(id));
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            await _projectSettingService.Process(new RestoreProject(id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanently(string id)
        {
            await _projectSettingService.Process(new DeleteProjectPermanently(id));
            return NoContent();
        }
    }
}