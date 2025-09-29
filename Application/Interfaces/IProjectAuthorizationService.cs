namespace Application.Interfaces
{
    public interface IProjectAuthorizationService
    {
        Task<bool> CanUserDeleteProjectAsync(Guid projectId, Guid userId);
        Task<bool> CanUserUpdateProjectAsync(Guid projectId, Guid userId);
        Task<AuthorizationResult> ValidateProjectDeletionAsync(Guid projectId, Guid userId);
        Task<AuthorizationResult> ValidateProjectUpdateAsync(Guid projectId, Guid userId);
    }

    public class AuthorizationResult
    {
        public bool IsAuthorized { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthorizationResult Success() => new() { IsAuthorized = true };
        public static AuthorizationResult Failure(string errorMessage) => new() { IsAuthorized = false, ErrorMessage = errorMessage };
    }
}