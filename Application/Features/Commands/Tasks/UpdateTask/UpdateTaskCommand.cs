namespace Application.Features.Commands.Tasks.UpdateTask
{
    using Application.DTOs;
    using MediatR;

    public class UpdateTaskCommand : IRequest<bool>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }
        public TaskDTO Task { get; }
        public Guid UserId { get; }

        public UpdateTaskCommand(Guid projectId, Guid taskId, TaskDTO task, Guid userId)
        {
            ProjectId = projectId;
            TaskId = taskId;
            Task = task;
            UserId = userId;
        }
    }
}
