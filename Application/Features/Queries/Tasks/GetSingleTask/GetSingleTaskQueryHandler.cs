using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQueryHandler : IRequestHandler<GetSingleTaskQuery, ProjectTask>
    {
        private readonly ITaskRepository _taskRepository;
        public GetSingleTaskQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ProjectTask> Handle(GetSingleTaskQuery request, CancellationToken cancellationToken)
        {
            return await _taskRepository.GetTaskByIdAsync(request.ProjectId, request.TaskId);
        }
    }
}
