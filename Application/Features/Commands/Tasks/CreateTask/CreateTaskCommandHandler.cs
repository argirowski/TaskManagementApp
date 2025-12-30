using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Tasks.CreateTask
{

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger<CreateTaskCommandHandler> _logger;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper, IProjectAuthorizationService authorizationService, ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskDTO?> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating task '{TaskTitle}' for project {ProjectId} by user {UserId}", request.Task.ProjectTaskTitle, request.ProjectId, request.UserId);

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                _logger.LogWarning("Task creation attempted for non-existent project {ProjectId} by user {UserId}", request.ProjectId, request.UserId);
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                _logger.LogWarning("Task creation denied: user {UserId} is not the owner of project {ProjectId}", request.UserId, request.ProjectId);
                throw new ForbiddenException("You do not have permission to create a task for this project.");
            }
            // Check for duplicate task name within the project
            var exists = await _taskRepository.ExistsByNameAsync(request.ProjectId, request.Task.ProjectTaskTitle);
            if (exists)
            {
                _logger.LogWarning("Task creation failed: duplicate task name '{TaskTitle}' in project {ProjectId} for user {UserId}", request.Task.ProjectTaskTitle, request.ProjectId, request.UserId);
                throw new BadRequestException($"A task with the name '{request.Task.ProjectTaskTitle}' already exists.");
            }

            var newTask = _mapper.Map<ProjectTask>(request.Task);
            newTask.ProjectId = request.ProjectId;
            newTask.Id = Guid.NewGuid();

            var createdTask = await _taskRepository.CreateTaskAsync(newTask);
            // Check if creation was successful
            if (createdTask == null)
            {
                _logger.LogError("Failed to create task '{TaskTitle}' for project {ProjectId} by user {UserId} - repository returned null", request.Task.ProjectTaskTitle, request.ProjectId, request.UserId);
                throw new BadRequestException("Failed to create the task. Please try again.");
            }

            _logger.LogInformation("Successfully created task '{TaskTitle}' with ID {TaskId} for project {ProjectId} by user {UserId}", createdTask.ProjectTaskTitle, createdTask.Id, request.ProjectId, request.UserId);
            return _mapper.Map<TaskDTO>(createdTask);
        }
    }
}
