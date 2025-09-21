namespace Application.Features.Commands.Tasks.UpdateTask
{
    using MediatR;
    public class UpdateTaskCommand : IRequest<bool>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }
        public string Title { get; }
        public string Description { get; }

        public UpdateTaskCommand(Guid projectId, Guid taskId, string title, string description)
        {
            ProjectId = projectId;
            TaskId = taskId;
            Title = title;
            Description = description;
        }
    }
}
