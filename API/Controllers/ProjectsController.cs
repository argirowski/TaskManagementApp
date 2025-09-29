using System.Security.Claims;
using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Features.Commands.Projects.DeleteProject;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Features.Queries.Projects.GetAllProjects;
using Application.Features.Queries.Projects.GetSingleProject;
using Application.Interfaces;
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
        private readonly IProjectAuthorizationService _authorizationService;

        public ProjectsController(IMediator mediator, IProjectAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ProjectDTO>>> GetAllProjects()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectDetailsDTO>> GetSingleProject(Guid id)
        {
            var projectDetailsDTO = await _mediator.Send(new GetSingleProjectQuery(id));
            if (projectDetailsDTO == null)
                return NotFound();
            return Ok(projectDetailsDTO);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var validationResult = await _authorizationService.ValidateProjectDeletionAsync(id, userId.Value);
            if (!validationResult.IsAuthorized)
                return Forbid();

            await _mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateProjectDTO>> CreateProject([FromBody] CreateProjectDTO createProjectDTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var command = new CreateProjectCommand(createProjectDTO, userId.Value);
            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest("Project creation failed.");

            return StatusCode(201, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] CreateProjectDTO editProjectDTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var validationResult = await _authorizationService.ValidateProjectUpdateAsync(id, userId.Value);
            if (!validationResult.IsAuthorized)
                return Forbid();

            var command = new UpdateProjectCommand(id, editProjectDTO);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
