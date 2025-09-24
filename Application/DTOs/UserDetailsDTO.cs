namespace Application.DTOs
{
    public class UserDetailsDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
    }
}
