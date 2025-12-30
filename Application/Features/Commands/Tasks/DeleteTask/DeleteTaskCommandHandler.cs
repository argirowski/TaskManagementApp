using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger<DeleteTaskCommandHandler> _logger;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IProjectAuthorizationService authorizationService, ILogger<DeleteTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting task {TaskId} from project {ProjectId} for user {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Check if user is authenticated
            if (request.UserId == Guid.Empty)
            {
                _logger.LogWarning("Task deletion attempted without authenticated user ID for task {TaskId} in project {ProjectId}", request.TaskId, request.ProjectId);
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                _logger.LogWarning("Task deletion attempted for non-existent project {ProjectId} by user {UserId}", request.ProjectId, request.UserId);
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                _logger.LogWarning("Task deletion denied: user {UserId} is not the owner of project {ProjectId}", request.UserId, request.ProjectId);
                throw new ForbiddenException("You do not have permission to delete this task.");
            }
            // Check if task exists in the specified project
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                _logger.LogWarning("Task deletion attempted for non-existent task {TaskId} in project {ProjectId} by user {UserId}", request.TaskId, request.ProjectId, request.UserId);
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }
            // Delete the task
            var deleted = await _taskRepository.DeleteTaskAsync(request.ProjectId, request.TaskId);
            if (!deleted)
            {
                _logger.LogError("Failed to delete task {TaskId} from project {ProjectId} for user {UserId} - repository returned false", request.TaskId, request.ProjectId, request.UserId);
                throw new BadRequestException("Failed to delete the task. Please try again.");
            }

            _logger.LogInformation("Successfully deleted task {TaskId} (Name: '{TaskTitle}') from project {ProjectId} for user {UserId}", request.TaskId, task.ProjectTaskTitle, request.ProjectId, request.UserId);
            return true;
        }
    }
}
