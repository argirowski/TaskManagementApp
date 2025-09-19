namespace Application.Features.Commands.Tasks.DeleteTask
{
    using MediatR;
    public class DeleteTaskCommand : IRequest<bool>
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
