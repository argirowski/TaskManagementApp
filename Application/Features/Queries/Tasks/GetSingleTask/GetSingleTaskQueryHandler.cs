using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using AutoMapper;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQueryHandler : IRequestHandler<GetSingleTaskQuery, TaskDTO?>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetSingleTaskQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskDTO?> Handle(GetSingleTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
            if (task == null)
            {
                return null;
            }

            return _mapper.Map<TaskDTO>(task);
        }
    }
}
