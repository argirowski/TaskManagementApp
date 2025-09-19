using Application.DTOs;
using Application.Features.Queries.Tasks.GetAllTasks;
using Application.Features.Queries.Tasks.GetSingleTask;
using AutoMapper;
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

        public TasksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
    }
}
