namespace Application.DTOs
{
    public class TaskDetailsDTO
    {
        public Guid Id { get; set; }
        public required string ProjectTaskTitle { get; set; }
        public string ProjectTaskDescription { get; set; } = null!;
    }
}
