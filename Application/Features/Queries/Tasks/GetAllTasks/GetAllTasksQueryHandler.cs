using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.Tasks.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
            }

            var tasks = await _taskRepository.GetTasksByProjectIdAsync(request.ProjectId);
            return _mapper.Map<List<TaskDTO>>(tasks);
        }
    }
}
