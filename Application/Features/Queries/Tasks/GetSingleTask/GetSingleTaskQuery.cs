namespace Application.Features.Queries.Tasks.GetSingleTask
{
    using MediatR;
    using Domain.Entities;
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
