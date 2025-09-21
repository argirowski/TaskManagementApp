using System.Security.Claims;
using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using Application.Features.Commands.Tasks.DeleteTask;
using Application.Features.Commands.Tasks.UpdateTask;
using Application.Features.Queries.Tasks.GetAllTasks;
using Application.Features.Queries.Tasks.GetSingleTask;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public TasksController(IMediator mediator, IMapper mapper, IProjectRepository projectRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<TaskDTO>>> GetAllTasksForProject(Guid projectId)
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery(projectId));
            var taskDTOs = _mapper.Map<List<TaskDTO>>(tasks);
            return Ok(taskDTOs);
        }

        [HttpGet("project/{projectId}/task/{taskId}")]
        public async Task<ActionResult<TaskDTO>> GetSingleTaskForProject(Guid projectId, Guid taskId)
        {
            var task = await _mediator.Send(new GetSingleTaskQuery(projectId, taskId));
            if (task == null)
                return NotFound();
            var taskDTO = _mapper.Map<TaskDTO>(task);
            return Ok(taskDTO);
        }
        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] TaskDTO taskDTO)
        {
            var command = new CreateTaskCommand(
                projectId,
                taskDTO.ProjectTaskTitle ?? string.Empty,
                taskDTO.ProjectTaskDescription ?? string.Empty
            );
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Task creation failed.");
            return Ok("Task created successfully.");
        }

        [HttpDelete("project/{projectId}/task/{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var role = await _projectRepository.GetUserRoleAsync(projectId, userId);
            if (role != ProjectRole.Owner)
                return Forbid();

            var command = new DeleteTaskCommand(projectId, taskId);
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPut("project/{projectId}/task/{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, [FromBody] TaskDTO taskDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var role = await _projectRepository.GetUserRoleAsync(projectId, userId);
            if (role != ProjectRole.Owner)
                return Forbid();

            var command = new UpdateTaskCommand(
                projectId,
                taskId,
                taskDTO.ProjectTaskTitle ?? string.Empty,
                taskDTO.ProjectTaskDescription ?? string.Empty
            );
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();
            return Ok("Task updated successfully.");
        }
    }
}
