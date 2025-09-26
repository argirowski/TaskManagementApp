using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.Projects.GetSingleProject
{
    public class GetSingleProjectQuery
    : IRequest<ProjectDetailsDTO?>
    {
        public Guid Id { get; set; }
        public GetSingleProjectQuery(Guid id)
        {
            Id = id;
        }
    }
}
