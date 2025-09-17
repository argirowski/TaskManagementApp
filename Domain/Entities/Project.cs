namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Relationships
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    }
}

