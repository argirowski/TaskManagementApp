using MediatR;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }
        public Guid UserId { get; }

        public DeleteTaskCommand(Guid projectId, Guid taskId, Guid userId)
        {
            ProjectId = projectId;
            TaskId = taskId;
            UserId = userId;
        }
    }
}
