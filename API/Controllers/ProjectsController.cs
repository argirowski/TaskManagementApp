using System.Security.Claims;
using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Features.Commands.Projects.DeleteProject;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Features.Queries.Projects.GetAllProjects;
using Application.Features.Queries.Projects.GetSingleProject;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IMediator mediator, IMapper mapper, IProjectRepository projectRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> GetAllProjects()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDTO>> GetSingleProject(Guid id)
        {
            var project = await _mediator.Send(new GetSingleProjectQuery(id));
            if (project == null)
                return NotFound();
            var detailsDTO = _mapper.Map<ProjectDetailsDTO>(project);
            return Ok(detailsDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var role = await _projectRepository.GetUserRoleAsync(id, userId);
            if (role != Domain.Enums.ProjectRole.Owner)
                return Forbid();

            var result = await _mediator.Send(new DeleteProjectCommand(id));
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO projectDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var command = new CreateProjectCommand(projectDTO, userId);
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Project creation failed.");
            return Ok("Project created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDTO projectDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var role = await _projectRepository.GetUserRoleAsync(id, userId);
            if (role != Domain.Enums.ProjectRole.Owner)
                return Forbid();

            var command = new UpdateProjectCommand(id, projectDTO);
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();
            return Ok("Project updated successfully.");
        }
    }
}
