namespace Application.Features.Commands.Tasks.UpdateTask
{
    using MediatR;
    using Domain.Interfaces;
    using AutoMapper;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);

            if (task == null)
            {
                throw new InvalidOperationException("Task not found.");
            }

            _mapper.Map(request.Task, task);
            await _taskRepository.UpdateTaskAsync(task);
            return Unit.Value;
        }
    }
}
