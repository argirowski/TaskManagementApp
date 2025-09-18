namespace Application.DTOs
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
