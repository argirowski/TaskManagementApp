namespace Application.DTOs
{
    public class ProjectDetailsDTO
    {
        public string ProjectName { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public List<UserDTO> Users { get; set; } = new();
        public List<TaskDTO> Tasks { get; set; } = new();
    }
}
