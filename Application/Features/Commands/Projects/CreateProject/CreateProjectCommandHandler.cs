using Application.DTOs;
using Application.Exceptions;
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
            // Check if userId is authenticated
            if (request.UserId == Guid.Empty)
            {
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UnauthorizedException("User with the specified ID does not exist.");
            }

            // Check for duplicate project name
            var exists = await _projectRepository.ProjectExistsByNameAsync(request.Project.ProjectName);
            if (exists)
            {
                throw new BadRequestException($"A project with the name '{request.Project.ProjectName}' already exists.");
            }

            var project = _mapper.Map<Project>(request.Project);
            project.Id = Guid.NewGuid();
            var created = await _projectRepository.CreateProjectAsync(project, request.UserId);
            if (created == null)
            {
                throw new BadRequestException("Failed to create the project. Please try again.");
            }

            return _mapper.Map<CreateProjectDTO>(created);
        }
    }
}
