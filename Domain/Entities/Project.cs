namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }

        // Relationships
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    }
}

