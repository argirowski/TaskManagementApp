namespace Application.Features.Commands.Tasks.UpdateTask
{
    using MediatR;
    using Domain.Interfaces;
    using AutoMapper;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                return false;
            }

            _mapper.Map(request.Task, task);
            var updated = await _taskRepository.UpdateTaskAsync(task);
            return updated;
        }
    }
}
