using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger<DeleteProjectCommandHandler> _logger;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository, IProjectAuthorizationService authorizationService, ILogger<DeleteProjectCommandHandler> logger)
        {
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting project {ProjectId} for user {UserId}", request.Id, request.UserId);

            // Check if user is authenticated
            if (request.UserId == Guid.Empty)
            {
                _logger.LogWarning("Project deletion attempted without authenticated user ID for project {ProjectId}", request.Id);
                throw new UnauthorizedException("No user ID provided. User must be authenticated.");
            }

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                _logger.LogWarning("Project deletion attempted for non-existent project {ProjectId} by user {UserId}", request.Id, request.UserId);
                throw new NotFoundException($"Project with ID {request.Id} not found.");
            }

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
            {
                _logger.LogWarning("Project deletion denied: user {UserId} is not the owner of project {ProjectId}", request.UserId, request.Id);
                throw new ForbiddenException("You do not have permission to delete this project.");
            }

            // Delete the project
            var deleted = await _projectRepository.DeleteProjectAsync(request.Id);
            if (!deleted)
            {
                _logger.LogError("Failed to delete project {ProjectId} for user {UserId} - repository returned false", request.Id, request.UserId);
                throw new BadRequestException("Failed to delete the project. Please try again.");
            }

            _logger.LogInformation("Successfully deleted project {ProjectId} (Name: '{ProjectName}') for user {UserId}", request.Id, project.ProjectName, request.UserId);
            return true;
        }
    }
}
