using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Tasks.CreateTask
{
    public class CreateTaskCommand : IRequest<TaskDTO?>
    {
        public Guid ProjectId { get; }
        public TaskDTO Task { get; }

        public CreateTaskCommand(Guid projectId, TaskDTO task)
        {
            ProjectId = projectId;
            Task = task;
        }
    }
}