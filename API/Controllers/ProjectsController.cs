using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Features.Commands.Projects.DeleteProject;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Features.Queries.Projects.GetAllProjects;
using Application.Features.Queries.Projects.GetSingleProject;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedResultDTO<ProjectDTO>>> GetAllProjects([FromQuery] PaginationParams paginationParams)
        {
            var query = new GetAllProjectsQuery { Page = paginationParams.PageNumber, PageSize = paginationParams.PageSize };
            var projects = await _mediator.Send(query);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectDetailsDTO>> GetSingleProject([FromRoute] Guid id)
        {
            var projectDetailsDTO = await _mediator.Send(new GetSingleProjectQuery(id));
            return Ok(projectDetailsDTO);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            await _mediator.Send(new DeleteProjectCommand(id, userId ?? Guid.Empty));
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateProjectDTO>> CreateProject([FromBody] CreateProjectDTO createProjectDTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var command = new CreateProjectCommand(createProjectDTO, userId.Value);
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Project creation failed.");
            }

            return StatusCode(201, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject([FromRoute] Guid id, [FromBody] CreateProjectDTO editProjectDTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var command = new UpdateProjectCommand(id, editProjectDTO, userId.Value);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return Forbid();
            }

            return NoContent();
        }
    }
}
