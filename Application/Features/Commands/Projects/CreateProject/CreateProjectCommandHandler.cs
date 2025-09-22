using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate project name
            var exists = await _projectRepository.ExistsByNameAsync(request.Project.ProjectName);
            if (exists)
                return false;

            var project = _mapper.Map<Project>(request.Project);
            project.Id = Guid.NewGuid();
            var created = await _projectRepository.CreateAsync(project, request.UserId);
            return created != null;
        }
    }
}
