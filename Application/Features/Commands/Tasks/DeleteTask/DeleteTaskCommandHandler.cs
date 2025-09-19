namespace Application.Features.Commands.Tasks.DeleteTask
{
    using MediatR;
    using Domain.Interfaces;
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return await _taskRepository.DeleteTaskAsync(request.ProjectId, request.TaskId);
        }
    }
}
