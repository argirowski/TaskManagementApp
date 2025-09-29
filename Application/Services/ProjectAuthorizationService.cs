using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProjectAuthorizationService : IProjectAuthorizationService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectAuthorizationService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> CanUserDeleteProjectAsync(Guid projectId, Guid userId)
        {
            var role = await _projectRepository.GetUserRoleAsync(projectId, userId);
            return role == ProjectRole.Owner;
        }

        public async Task<bool> CanUserUpdateProjectAsync(Guid projectId, Guid userId)
        {
            var role = await _projectRepository.GetUserRoleAsync(projectId, userId);
            return role == ProjectRole.Owner;
        }

        public async Task<AuthorizationResult> ValidateProjectDeletionAsync(Guid projectId, Guid userId)
        {
            var canDelete = await CanUserDeleteProjectAsync(projectId, userId);
            return canDelete
                ? AuthorizationResult.Success()
                : AuthorizationResult.Failure("You don't have permission to delete this project.");
        }

        public async Task<AuthorizationResult> ValidateProjectUpdateAsync(Guid projectId, Guid userId)
        {
            var canUpdate = await CanUserUpdateProjectAsync(projectId, userId);
            return canUpdate
                ? AuthorizationResult.Success()
                : AuthorizationResult.Failure("You don't have permission to update this project.");
        }
    }
}