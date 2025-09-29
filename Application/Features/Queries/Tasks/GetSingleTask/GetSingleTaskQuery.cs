using MediatR;
using Domain.Entities;
using Application.DTOs;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQuery : IRequest<TaskDTO?>
    {
        public Guid ProjectId { get; }
        public Guid TaskId { get; }

        public GetSingleTaskQuery(Guid projectId, Guid taskId)
        {
            ProjectId = projectId;
            TaskId = taskId;
        }
    }
}
