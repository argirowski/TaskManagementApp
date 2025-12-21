using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQueryHandler : IRequestHandler<GetSingleTaskQuery, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetSingleTaskQueryHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<TaskDTO?> Handle(GetSingleTaskQuery request, CancellationToken cancellationToken)
        {
            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }

            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");
            }

            return _mapper.Map<TaskDTO>(task);
        }
    }
}
