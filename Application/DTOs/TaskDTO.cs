namespace Application.DTOs
{
    public class TaskDTO
    {
        public required string ProjectTaskTitle { get; set; }
        public string ProjectTaskDescription { get; set; } = null!;
    }
}
