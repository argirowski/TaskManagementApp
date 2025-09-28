namespace Application.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
    }
}
