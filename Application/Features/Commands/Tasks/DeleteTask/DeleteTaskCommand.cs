using MediatR;

namespace Application.Features.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }

        public DeleteTaskCommand(Guid projectId, Guid taskId)
        {
            ProjectId = projectId;
            TaskId = taskId;
        }
    }
}
