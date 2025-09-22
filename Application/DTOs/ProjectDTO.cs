namespace Application.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string ProjectDescription { get; set; } = null!;
    }
}
