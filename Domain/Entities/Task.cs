namespace Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Foreign Keys
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public Guid? AssignedUserId { get; set; } // nullable (unassigned possible)
        public User? AssignedUser { get; set; }
    }
}
