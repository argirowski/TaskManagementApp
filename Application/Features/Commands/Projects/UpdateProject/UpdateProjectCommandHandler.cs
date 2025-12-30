using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProjectCommandHandler> _logger;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IProjectAuthorizationService authorizationService, IMapper mapper, ILogger<UpdateProjectCommandHandler> logger)
        {
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating project {ProjectId} for user {UserId}", request.Id, request.UserId);

            // Check if userId is empty
            if (request.UserId == Guid.Empty)
            {
                _logger.LogWarning("Project update attempted without authenticated user ID for project {ProjectId}", request.Id);
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            // Check if project exists
            if (project == null)
            {
                _logger.LogWarning("Project update attempted for non-existent project {ProjectId} by user {UserId}", request.Id, request.UserId);
                throw new NotFoundException($"Project with ID {request.Id} not found.");
            }

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
            {
                _logger.LogWarning("Project update denied: user {UserId} is not the owner of project {ProjectId}", request.UserId, request.Id);
                throw new ForbiddenException("You do not have permission to update this project.");
            }

            // Map updated fields
            _mapper.Map(request.Project, project);
            var updated = await _projectRepository.UpdateProjectAsync(project);
            // Check if update was successful
            if (!updated)
            {
                _logger.LogError("Failed to update project {ProjectId} for user {UserId} - repository returned false", request.Id, request.UserId);
                throw new BadRequestException("Failed to update the project. Please try again.");
            }

            _logger.LogInformation("Successfully updated project {ProjectId} (Name: '{ProjectName}') for user {UserId}", request.Id, project.ProjectName, request.UserId);
            return true;
        }
    }
}
