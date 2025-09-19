using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.Projects.GetSingleProject
{
    public class GetSingleProjectQueryHandler : IRequestHandler<GetSingleProjectQuery, Project?>
    {
        private readonly IProjectRepository _projectRepository;

        public GetSingleProjectQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project?> Handle(GetSingleProjectQuery request, CancellationToken cancellationToken)
        {
            return await _projectRepository.GetByIdAsync(request.Id);
        }
    }
}
