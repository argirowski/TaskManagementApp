namespace Application.Features.Commands.Tasks.UpdateTask
{
    using Application.DTOs;
    using MediatR;

    public class UpdateTaskCommand : IRequest<Unit>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }
        public TaskDTO Task { get; }

        public UpdateTaskCommand(Guid projectId, Guid taskId, TaskDTO task)
        {
            ProjectId = projectId;
            TaskId = taskId;
            Task = task;
        }
    }
}
