using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _authorizationService;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository, IProjectAuthorizationService authorizationService)
        {
            _projectRepository = projectRepository;
            _authorizationService = authorizationService;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            // Check if user is authenticated
            if (request.UserId == Guid.Empty)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {request.Id} not found.");
            }

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
            {
                throw new ForbiddenException("You do not have permission to delete this project.");
            }

            // Delete the project
            var deleted = await _projectRepository.DeleteProjectAsync(request.Id);
            return deleted;
        }
    }
}
