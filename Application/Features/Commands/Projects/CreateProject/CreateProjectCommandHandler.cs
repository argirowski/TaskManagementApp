using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectDTO?>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CreateProjectDTO?> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return null;
            }

            // Check for duplicate project name
            var exists = await _projectRepository.ExistsByNameAsync(request.Project.ProjectName);
            if (exists)
            {
                return null;
            }

            var project = _mapper.Map<Project>(request.Project);
            project.Id = Guid.NewGuid();
            var created = await _projectRepository.CreateAsync(project, request.UserId);
            if (created == null)
            {
                return null;
            }

            return _mapper.Map<CreateProjectDTO>(created);
        }
    }
}
