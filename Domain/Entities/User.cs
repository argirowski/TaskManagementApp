namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        // Relationships
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectTask> AssignedProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
