using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.Projects.GetSingleProject
{
    public class GetSingleProjectQueryHandler : IRequestHandler<GetSingleProjectQuery, ProjectDetailsDTO?>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetSingleProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDetailsDTO?> Handle(GetSingleProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
                return null;
            return _mapper.Map<ProjectDetailsDTO>(project);
        }
    }
}
