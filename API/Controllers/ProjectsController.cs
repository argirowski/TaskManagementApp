using System.Security.Claims;
using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Features.Commands.Projects.DeleteProject;
using Application.Features.Queries.Projects.GetAllProjects;
using Application.Features.Queries.Projects.GetSingleProject;
using AutoMapper;
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

        public ProjectsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> GetAll()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDTO>> GetSingle(Guid id)
        {
            var project = await _mediator.Send(new GetSingleProjectQuery(id));
            if (project == null)
                return NotFound();
            var detailsDTO = _mapper.Map<ProjectDetailsDTO>(project);
            return Ok(detailsDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id));
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDTO projectDTO)
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
    }
}
