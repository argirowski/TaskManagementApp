namespace Application.Features.Queries.Tasks.GetAllTasks
{
    using Application.DTOs;
    using MediatR;
    public class GetAllTasksQuery : IRequest<List<TaskDTO>>
    {
        public Guid ProjectId { get; }
        public GetAllTasksQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
