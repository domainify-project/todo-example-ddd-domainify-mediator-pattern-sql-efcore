using Microsoft.AspNetCore.Mvc;
using Domainify.AspMvc;
using Contract;
using Domainify.Domain;
using Domain.ProjectSetting;

namespace Presentation.WebAPI
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProjectsController : ApiController
    {
        private readonly IProjectSettingService _service;

        public ProjectsController(
            IProjectSettingService projectService)
        {
            _service = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProjectViewModel>>> GetList()
        {
            var request = GetRequest<GetProjectsList>();
            return await _service.Process(request);
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

            return await _service.Process(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModel?>> Get(
            string id, bool? withSprints = null, bool? withTasks = null)
        {
            var request = GetRequest<GetProject>();
            request.SetId(id);

            return await _service.Process(request);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectViewModel?>> Define(DefineProject request)
        {
            var id = await _service.Process(request);
            return StatusCode(201, id);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeName(ChangeProjectName request)
        {
            await _service.Process(request);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Process(new DeleteProject(id));
            return NoContent();
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> CheckProjectForDeletingPermanently(string id)
        {
            await _service.Process(new CheckProjectForDeleting(id));
            return NoContent();
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            await _service.Process(new RestoreProject(id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanently(string id)
        {
            await _service.Process(new DeleteProjectPermanently(id));
            return NoContent();
        }
    }
}