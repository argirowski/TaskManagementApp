using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Tasks.CreateTask
{
    public class CreateTaskCommand : IRequest<TaskDTO?>
    {
        public Guid ProjectId { get; }
        public TaskDTO Task { get; }
        public Guid UserId { get; }

        public CreateTaskCommand(Guid projectId, TaskDTO task, Guid userId)
        {
            ProjectId = projectId;
            Task = task;
            UserId = userId;
        }
    }
}