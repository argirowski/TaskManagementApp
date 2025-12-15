using MediatR;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectAuthorizationService _authorizationService;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IProjectAuthorizationService authorizationService)
        {
            _taskRepository = taskRepository;
            _authorizationService = authorizationService;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                return false;
            }

            // Check if task exists in the specified project
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                return false;
            }
            // Delete the task
            var deleted = await _taskRepository.DeleteTaskAsync(request.ProjectId, request.TaskId);
            return deleted;
        }
    }
}
