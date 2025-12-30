using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Application.Features.Queries.Projects.GetAllProjects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PagedResultDTO<ProjectDTO>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProjectsQueryHandler> _logger;

        public GetAllProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper, ILogger<GetAllProjectsQueryHandler> logger)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResultDTO<ProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all projects - Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

            var (projects, totalProjects) = await _projectRepository.GetAllProjectsAsync(request.Page, request.PageSize);

            _logger.LogInformation("Successfully retrieved {Count} projects (Total: {TotalCount}) - Page: {Page}, PageSize: {PageSize}", 
                projects.Count(), totalProjects, request.Page, request.PageSize);

            return new PagedResultDTO<ProjectDTO>
            {
                Items = _mapper.Map<List<ProjectDTO>>(projects),
                TotalCount = totalProjects,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
