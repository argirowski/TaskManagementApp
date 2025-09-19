using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.Tasks.GetAllTasks
{
    public class GetAllTasksQuery : IRequest<List<TaskDTO>>
    {
        public Guid ProjectId { get; }
        public GetAllTasksQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
