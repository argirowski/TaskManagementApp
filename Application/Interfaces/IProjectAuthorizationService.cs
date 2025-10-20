using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectAuthorizationService
    {
        Task<bool> CanUserDeleteProjectAsync(Guid projectId, Guid userId);
        Task<bool> CanUserUpdateProjectAsync(Guid projectId, Guid userId);
        Task<AuthorizationResultDTO> ValidateProjectDeletionAsync(Guid projectId, Guid userId);
        Task<AuthorizationResultDTO> ValidateProjectUpdateAsync(Guid projectId, Guid userId);
    }
}