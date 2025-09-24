namespace Application.DTOs
{
    public class TaskDetailsDTO
    {
        public Guid Id { get; set; }
        public string ProjectTaskTitle { get; set; } = null!;
        public string ProjectTaskDescription { get; set; } = null!;
    }
}
