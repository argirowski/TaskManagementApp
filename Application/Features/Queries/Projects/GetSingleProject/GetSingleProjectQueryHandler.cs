using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Queries.Projects.GetSingleProject
{
    public class GetSingleProjectQueryHandler : IRequestHandler<GetSingleProjectQuery, ProjectDetailsDTO?>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSingleProjectQueryHandler> _logger;

        public GetSingleProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper, ILogger<GetSingleProjectQueryHandler> logger)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProjectDetailsDTO?> Handle(GetSingleProjectQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching project {ProjectId}", request.Id);

            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            // Check if project exists
            if (project == null)
            {
                _logger.LogWarning("Project not found: {ProjectId}", request.Id);
                throw new NotFoundException($"Project with ID {request.Id} not found.");
            }

            _logger.LogInformation("Successfully retrieved project {ProjectId} (Name: '{ProjectName}')", request.Id, project.ProjectName);
            return _mapper.Map<ProjectDetailsDTO>(project);
        }
    }
}
