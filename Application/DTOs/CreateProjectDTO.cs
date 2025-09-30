namespace Application.DTOs
{
    public class CreateProjectDTO
    {
        public required string ProjectName { get; set; }
        public string ProjectDescription { get; set; } = null!;
    }
}
