using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectTask
    {
        public Guid Id { get; set; }
        public required string ProjectTaskTitle { get; set; }
        public string? ProjectTaskDescription { get; set; }
        public TaskProgressStatus ProjectTaskStatus { get; set; } = TaskProgressStatus.ToDo;

        // Foreign Keys
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid? AssignedUserId { get; set; } // nullable (unassigned possible)
        public User? AssignedUser { get; set; }
    }
}
