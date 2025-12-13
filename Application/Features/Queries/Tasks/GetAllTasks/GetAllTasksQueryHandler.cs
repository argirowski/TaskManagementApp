using MediatR;
using Domain.Interfaces;
using Application.DTOs;
using AutoMapper;

namespace Application.Features.Queries.Tasks.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(request.ProjectId);
            return _mapper.Map<List<TaskDTO>>(tasks);
        }
    }
}
