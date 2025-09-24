namespace Application.DTOs
{
    public class ProjectDetailsDTO
    {
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public List<UserDetailsDTO> Users { get; set; } = new List<UserDetailsDTO>();
        public List<TaskDetailsDTO> Tasks { get; set; } = new List<TaskDetailsDTO>();
    }
}
