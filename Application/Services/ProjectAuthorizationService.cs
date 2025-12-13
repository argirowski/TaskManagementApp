using Application.DTOs;
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


        public async Task<bool> IsUserOwnerAsync(Guid projectId, Guid userId)
        {
            var role = await _projectRepository.GetUserRoleAsync(projectId, userId);
            return role == ProjectRole.Owner;
        }

        public async Task<AuthorizationResultDTO> ValidateProjectOwnerAsync(Guid projectId, Guid userId)
        {
            var isOwner = await IsUserOwnerAsync(projectId, userId);
            return isOwner
                ? AuthorizationResultDTO.Success()
                : AuthorizationResultDTO.Failure("You don't have permission for this operation");
        }
    }
}