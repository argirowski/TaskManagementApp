using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Queries.Tasks.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTasksQueryHandler> _logger;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper, ILogger<GetAllTasksQueryHandler> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all tasks for project {ProjectId}", request.ProjectId);

            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            // Check if project exists
            if (project == null)
            {
                _logger.LogWarning("Tasks fetch failed: project not found - {ProjectId}", request.ProjectId);
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }

            var tasks = await _taskRepository.GetTasksByProjectIdAsync(request.ProjectId);
            var tasksList = tasks.ToList();

            _logger.LogInformation("Successfully retrieved {Count} tasks for project {ProjectId} (Name: '{ProjectName}')", 
                tasksList.Count, request.ProjectId, project.ProjectName);

            return _mapper.Map<List<TaskDTO>>(tasksList);
        }
    }
}
