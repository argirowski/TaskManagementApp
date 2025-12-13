using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.DTOs;

namespace Application.Features.Commands.Tasks.CreateTask
{

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskDTO?> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate task name within the project
            var exists = await _taskRepository.ExistsByNameAsync(request.ProjectId, request.Task.ProjectTaskTitle);
            if (exists)
            {
                return null;
            }

            var newTask = _mapper.Map<ProjectTask>(request.Task);
            newTask.ProjectId = request.ProjectId;
            newTask.Id = Guid.NewGuid();

            var createdTask = await _taskRepository.CreateTaskAsync(newTask);
            if (createdTask == null)
            {
                return null;
            }

            return _mapper.Map<TaskDTO>(createdTask);
        }
    }
}
