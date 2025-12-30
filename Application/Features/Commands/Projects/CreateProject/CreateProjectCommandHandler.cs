using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Projects.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectDTO?>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProjectCommandHandler> _logger;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper, ILogger<CreateProjectCommandHandler> logger)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateProjectDTO?> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating project '{ProjectName}' for user {UserId}", request.Project.ProjectName, request.UserId);

            // Check if userId is authenticated
            if (request.UserId == Guid.Empty)
            {
                _logger.LogWarning("Project creation attempted without authenticated user ID");
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("Project creation attempted with non-existent user ID: {UserId}", request.UserId);
                throw new UnauthorizedException("User with the specified ID does not exist.");
            }

            // Check for duplicate project name
            var exists = await _projectRepository.ProjectExistsByNameAsync(request.Project.ProjectName);
            if (exists)
            {
                _logger.LogWarning("Project creation failed: duplicate project name '{ProjectName}' for user {UserId}", request.Project.ProjectName, request.UserId);
                throw new BadRequestException($"A project with the name '{request.Project.ProjectName}' already exists.");
            }

            var project = _mapper.Map<Project>(request.Project);
            project.Id = Guid.NewGuid();
            var created = await _projectRepository.CreateProjectAsync(project, request.UserId);
            if (created == null)
            {
                _logger.LogError("Failed to create project '{ProjectName}' for user {UserId} - repository returned null", request.Project.ProjectName, request.UserId);
                throw new BadRequestException("Failed to create the project. Please try again.");
            }

            _logger.LogInformation("Successfully created project '{ProjectName}' with ID {ProjectId} for user {UserId}", created.ProjectName, created.Id, request.UserId);

            return _mapper.Map<CreateProjectDTO>(created);
        }
    }
}
