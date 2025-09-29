using MediatR;
using Domain.Interfaces;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
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
