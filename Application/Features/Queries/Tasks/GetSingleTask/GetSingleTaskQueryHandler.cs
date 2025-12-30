using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQueryHandler : IRequestHandler<GetSingleTaskQuery, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSingleTaskQueryHandler> _logger;

        public GetSingleTaskQueryHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper, ILogger<GetSingleTaskQueryHandler> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskDTO?> Handle(GetSingleTaskQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching task {TaskId} from project {ProjectId}", request.TaskId, request.ProjectId);

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                _logger.LogWarning("Task fetch failed: project not found - {ProjectId}", request.ProjectId);
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }

            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            // Check if task exists
            if (task == null)
            {
                _logger.LogWarning("Task fetch failed: task not found - TaskId: {TaskId}, ProjectId: {ProjectId}", request.TaskId, request.ProjectId);
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }

            _logger.LogInformation("Successfully retrieved task {TaskId} (Name: '{TaskTitle}') from project {ProjectId} (Name: '{ProjectName}')", 
                request.TaskId, task.ProjectTaskTitle, request.ProjectId, project.ProjectName);
            return _mapper.Map<TaskDTO>(task);
        }
    }
}
