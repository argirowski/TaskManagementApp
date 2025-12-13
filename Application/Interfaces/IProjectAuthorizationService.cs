using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectAuthorizationService
    {
        Task<bool> IsUserOwnerAsync(Guid projectId, Guid userId);
        Task<AuthorizationResultDTO> ValidateProjectOwnerAsync(Guid projectId, Guid userId);
    }
}