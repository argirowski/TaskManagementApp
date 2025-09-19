using Domain.Entities;
using MediatR;

namespace Application.Features.Queries.Projects.GetSingleProject
{
    public class GetSingleProjectQuery
    : IRequest<Project?>
    {
        public Guid Id { get; set; }
        public GetSingleProjectQuery(Guid id)
        {
            Id = id;
        }
    }
}
