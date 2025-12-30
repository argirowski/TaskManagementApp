namespace Application.Features.Commands.Tasks.UpdateTask
{
    using Application.Exceptions;
    using Application.Interfaces;
    using AutoMapper;
    using Domain.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTaskCommandHandler> _logger;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IProjectAuthorizationService authorizationService, IMapper mapper, ILogger<UpdateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating task {TaskId} in project {ProjectId} for user {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Check if userId is empty
            if (request.UserId == Guid.Empty)
            {
                _logger.LogWarning("Task update attempted without authenticated user ID for task {TaskId} in project {ProjectId}", request.TaskId, request.ProjectId);
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }
            
            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                _logger.LogWarning("Task update attempted for non-existent project {ProjectId} by user {UserId}", request.ProjectId, request.UserId);
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }
            
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                _logger.LogWarning("Task update denied: user {UserId} is not the owner of project {ProjectId}", request.UserId, request.ProjectId);
                throw new ForbiddenException("You do not have permission to update this task.");
            }
            
            // Check if task exists in the specified project
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                _logger.LogWarning("Task update attempted for non-existent task {TaskId} in project {ProjectId} by user {UserId}", request.TaskId, request.ProjectId, request.UserId);
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }

            _mapper.Map(request.Task, task);
            var updated = await _taskRepository.UpdateTaskAsync(task);
            // Check if update was successful
            if (!updated)
            {
                _logger.LogError("Failed to update task {TaskId} in project {ProjectId} for user {UserId} - repository returned false", request.TaskId, request.ProjectId, request.UserId);
                throw new BadRequestException("Failed to update the task. Please try again.");
            }

            _logger.LogInformation("Successfully updated task {TaskId} (Name: '{TaskTitle}') in project {ProjectId} for user {UserId}", request.TaskId, task.ProjectTaskTitle, request.ProjectId, request.UserId);
            return updated;
        }
    }
}
