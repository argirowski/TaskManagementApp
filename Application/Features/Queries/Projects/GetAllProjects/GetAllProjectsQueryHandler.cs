using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.Projects.GetAllProjects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PagedResultDTO<ProjectDTO>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetAllProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<ProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var (projects, totalProjects) = await _projectRepository.GetAllProjectsAsync(request.Page, request.PageSize);

            return new PagedResultDTO<ProjectDTO>
            {
                Items = _mapper.Map<List<ProjectDTO>>(projects),
                TotalCount = totalProjects,
                Page = request.Page,
                PageSize = request.PageSize
            };
            //var projects = await _projectRepository.GetAllProjectsAsync();
            //var totalProjects = projects.Count;
            //var pagedProjects = projects
            //    .Skip((request.Page - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //    .ToList();

            //return new PagedResultDTO<ProjectDTO>
            //{
            //    Items = _mapper.Map<List<ProjectDTO>>(pagedProjects),
            //    TotalCount = totalProjects,
            //    Page = request.Page,
            //    PageSize = request.PageSize
            //};
        }
    }
}
