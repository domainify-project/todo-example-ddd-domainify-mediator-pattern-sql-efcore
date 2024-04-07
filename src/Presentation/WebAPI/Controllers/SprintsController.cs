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
        private readonly IProjectSettingService _projectSettingService;

        public SprintsController(IProjectSettingService service)
        {
            _projectSettingService = service;
        }
        [HttpGet($"/v1/{nameof(ProjectsController)}/{{{nameof(GetSprintsList.ProjectId)}}}/[controller]")]
        public async Task<ActionResult<PaginatedList<SprintViewModel>>> GetList(
            string projectId)
        {
            var request = GetRequest<GetSprintsList>()
                           .SetProjectId(projectId);
            return await _projectSettingService.Process(request);
        }
        [HttpGet($"/v1.1/{nameof(ProjectsController)}/{{{nameof(GetSprintsList.ProjectId)}}}/[controller]")]
        public async Task<ActionResult<PaginatedList<SprintViewModel>>> GetList(
            string projectId, 
            int? pageNumber = null,
            int? pageSize = null)
        {
            var request = GetRequest<GetSprintsList>()
                           .SetProjectId(projectId);
            request.Setup(
                paginationSetting: new PaginationSetting(
                    defaultPageNumber: 1, defaultPageSize: 10));

            return await _projectSettingService.Process(request);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SprintViewModel?>> Get(string id)
        {
            return await View(
                () => _projectSettingService.Process(new GetSprint(id)));
        }

        [HttpPost]
        public async Task<ActionResult<SprintViewModel?>> Define(DefineSprint request)
        {
            var id = await _projectSettingService.Process(request);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeSprintName(ChangeSprintName request)
        {
            return await View(
                () => _projectSettingService.Process(request));
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangeSprintTimeSpan(ChangeSprintTimeSpan request)
        {
            return await View(
                () => _projectSettingService.Process(request));
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Delete(
            string id, bool archivingAllTaskMode)
        {
            var request = GetRequest<DeleteSprint>();
            request.SetId(id);

            return await View(
                () => _projectSettingService.Process(request));
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> CheckSprintForDeletingPermanently(string id)
        {
            return await View(
                () => _projectSettingService.Process(new CheckSprintForDeletingPermanently(id)));
        }
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> Restore(string id)
        {
            return await View(
                () => _projectSettingService.Process(new RestoreSprint(id)));
        }
    }
}