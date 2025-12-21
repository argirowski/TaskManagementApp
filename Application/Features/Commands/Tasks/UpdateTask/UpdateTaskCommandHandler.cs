namespace Application.Features.Commands.Tasks.UpdateTask
{
    using Application.Exceptions;
    using Application.Interfaces;
    using AutoMapper;
    using Domain.Interfaces;
    using MediatR;

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
            // Check if userId is empty
            if (request.UserId == Guid.Empty)
            {
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                throw new ForbiddenException("You do not have permission to update this task.");
            }
            // Check if task exists in the specified project
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }

            _mapper.Map(request.Task, task);
            var updated = await _taskRepository.UpdateTaskAsync(task);
            // Check if update was successful
            if (!updated)
            {
                throw new BadRequestException("Failed to update the task. Please try again.");
            }

            return updated;
        }
    }
}
