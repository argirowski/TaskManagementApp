using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.Projects.GetAllProjects
{
    public class GetAllProjectsQuery
    : IRequest<PagedResultDTO<ProjectDTO>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
