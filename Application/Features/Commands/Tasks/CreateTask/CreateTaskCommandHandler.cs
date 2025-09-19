using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Commands.Tasks.CreateTask
{

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var newTask = new ProjectTask
            {
                ProjectId = request.ProjectId,
                ProjectTaskTitle = request.Title,
                ProjectTaskDescription = request.Description
            };
            return await _taskRepository.CreateTaskAsync(newTask);
        }
    }
}
