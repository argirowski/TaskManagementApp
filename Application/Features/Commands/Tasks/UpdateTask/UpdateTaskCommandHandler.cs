namespace Application.Features.Commands.Tasks.UpdateTask
{
    using MediatR;
    using Domain.Interfaces;
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;
        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);

            if (task == null)
            {
                throw new InvalidOperationException("Task not found.");
            }

            task.ProjectTaskTitle = request.Title;
            task.ProjectTaskDescription = request.Description;
            await _taskRepository.UpdateTaskAsync(task);
            return Unit.Value;
        }
    }
}
