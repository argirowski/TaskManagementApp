using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IProjectAuthorizationService authorizationService)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            // Check if user is authenticated
            if (request.UserId == Guid.Empty)
            {
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                throw new ForbiddenException("You do not have permission to delete this task.");
            }
            // Check if task exists in the specified project
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }
            // Delete the task
            var deleted = await _taskRepository.DeleteTaskAsync(request.ProjectId, request.TaskId);

            return deleted;
        }
    }
}
