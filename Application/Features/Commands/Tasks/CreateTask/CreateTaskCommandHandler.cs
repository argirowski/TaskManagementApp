using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Tasks.CreateTask
{

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IProjectAuthorizationService _authorizationService;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper, IProjectAuthorizationService authorizationService)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        public async Task<TaskDTO?> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }
            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.ProjectId, request.UserId);
            if (!isOwner)
            {
                throw new ForbiddenException("You do not have permission to create a task for this project.");
            }
            // Check for duplicate task name within the project
            var exists = await _taskRepository.ExistsByNameAsync(request.ProjectId, request.Task.ProjectTaskTitle);
            if (exists)
            {
                throw new BadRequestException($"A task with the name '{request.Task.ProjectTaskTitle}' already exists.");
            }

            var newTask = _mapper.Map<ProjectTask>(request.Task);
            newTask.ProjectId = request.ProjectId;
            newTask.Id = Guid.NewGuid();

            var createdTask = await _taskRepository.CreateTaskAsync(newTask);
            // Check if creation was successful
            if (createdTask == null)
            {
                throw new BadRequestException("Failed to create the task. Please try again.");
            }

            return _mapper.Map<TaskDTO>(createdTask);
        }
    }
}
