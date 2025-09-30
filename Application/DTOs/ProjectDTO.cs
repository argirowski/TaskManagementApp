namespace Application.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public required string ProjectName { get; set; }
        public string ProjectDescription { get; set; } = null!;
    }
}
