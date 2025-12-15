using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using Application.Features.Commands.Tasks.DeleteTask;
using Application.Features.Commands.Tasks.UpdateTask;
using Application.Features.Queries.Tasks.GetAllTasks;
using Application.Features.Queries.Tasks.GetSingleTask;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IProjectAuthorizationService _authorizationService;

        public TasksController(IMediator mediator, IProjectAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpGet("project/{projectId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllTasksForProject([FromRoute] Guid projectId)
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery(projectId));
            if (tasks == null)
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        [HttpGet("project/{projectId}/task/{taskId}")]
        [Authorize]
        public async Task<ActionResult<TaskDTO>> GetSingleTaskForProject([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var task = await _mediator.Send(new GetSingleTaskQuery(projectId, taskId));
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        [HttpPost("project/{projectId}")]
        [Authorize]
        public async Task<ActionResult<TaskDTO>> CreateTask([FromRoute] Guid projectId, [FromBody] TaskDTO taskDTO)
        {
            var command = new CreateTaskCommand(projectId, taskDTO);
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Task creation failed.");
            }
            return StatusCode(201, result);

        }

        [HttpDelete("project/{projectId}/task/{taskId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var command = new DeleteTaskCommand(projectId, taskId, userId.Value);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return Forbid();
            }

            return NoContent();
        }

        [HttpPut("project/{projectId}/task/{taskId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] TaskDTO taskDTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var command = new UpdateTaskCommand(projectId, taskId, taskDTO, userId.Value);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return Forbid();
            }

            return NoContent();
        }
    }
}
