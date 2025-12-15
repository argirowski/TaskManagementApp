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
            // Check if project exists
            var project = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                return false;
            }

            // Authorization check
            var isOwner = await _authorizationService.IsUserOwnerAsync(request.Id, request.UserId);
            if (!isOwner)
            {
                return false;
            }

            // Delete the project
            var deleted = await _projectRepository.DeleteProjectAsync(request.Id);
            return deleted;
        }
    }
}
