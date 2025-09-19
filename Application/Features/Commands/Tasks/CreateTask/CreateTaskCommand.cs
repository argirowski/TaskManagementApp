using MediatR;

namespace Application.Features.Commands.Tasks.CreateTask
{
    public class CreateTaskCommand : IRequest<bool>
    {
        public Guid ProjectId { get; }
        public string Title { get; }
        public string Description { get; }

        public CreateTaskCommand(Guid projectId, string title, string description)
        {
            ProjectId = projectId;
            Title = title;
            Description = description;
        }
    }
}