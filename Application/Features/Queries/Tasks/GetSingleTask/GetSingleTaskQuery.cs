using MediatR;
using Domain.Entities;

namespace Application.Features.Queries.Tasks.GetSingleTask
{
    public class GetSingleTaskQuery : IRequest<ProjectTask>
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
