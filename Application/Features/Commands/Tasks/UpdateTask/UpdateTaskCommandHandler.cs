namespace Application.Features.Commands.Tasks.UpdateTask
{
    using MediatR;
    using Domain.Interfaces;
    using AutoMapper;
    using Application.Interfaces;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IProjectAuthorizationService authorizationService, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                return false;
            }

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
