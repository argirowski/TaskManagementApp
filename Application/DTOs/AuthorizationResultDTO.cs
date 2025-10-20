namespace Application.DTOs
{
    public class AuthorizationResultDTO
    {
        public bool IsAuthorized { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthorizationResultDTO Success()
        {
            return new AuthorizationResultDTO { IsAuthorized = true };
        }

        public static AuthorizationResultDTO Failure(string errorMessage)
        {
            return new AuthorizationResultDTO { IsAuthorized = false, ErrorMessage = errorMessage };
        }
    }
}
