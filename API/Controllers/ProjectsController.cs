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
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize]
        public async Task<ActionResult<List<ProjectDTO>>> GetAllProjects()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ProjectDetailsDTO>> GetSingleProject(Guid id)
        {
            var projectDetailsDTO = await _mediator.Send(new GetSingleProjectQuery(id));
            if (projectDetailsDTO == null)
                return NotFound();
            return Ok(projectDetailsDTO);
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            //    return Unauthorized();

            //var role = await _projectRepository.GetUserRoleAsync(id, userId);
            //if (role != Domain.Enums.ProjectRole.Owner)
            //    return Forbid();

            await _mediator.Send(new DeleteProjectCommand(id));

            return NoContent();
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO createProjectDTO)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            //    return Unauthorized();
            var userId = Guid.Parse("F6B6C8D7-6A7F-9B8C-2D6E-6F7A8B9C1D2E"); // Temporary hardcoded user ID for testing

            var command = new CreateProjectCommand(createProjectDTO, userId);
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Project creation failed.");
            return Ok("Project created successfully.");
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] CreateProjectDTO editProjectDTO)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            //    return Unauthorized();

            //var role = await _projectRepository.GetUserRoleAsync(id, userId);
            //if (role != Domain.Enums.ProjectRole.Owner)
            //    return Forbid();

            var command = new UpdateProjectCommand(id, editProjectDTO);
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();
            return Ok("Project updated successfully.");
        }
    }
}
