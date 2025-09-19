namespace Application.Features.Queries.Tasks.GetAllTasks
{
    using MediatR;
    using Domain.Interfaces;
    using Application.DTOs;
    using AutoMapper;

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskDTO>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<List<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(request.ProjectId);
            return _mapper.Map<List<TaskDTO>>(tasks);
        }
    }
}
