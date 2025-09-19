namespace Application.DTOs
{
    public class ProjectDetailsDTO
    {
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
        public List<TaskDTO> Tasks { get; set; } = new List<TaskDTO>();
    }
}
