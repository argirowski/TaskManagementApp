using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.Projects.GetAllProjects
{
    public class GetAllProjectsQuery
    : IRequest<List<ProjectDTO>>
    {
    }
}
