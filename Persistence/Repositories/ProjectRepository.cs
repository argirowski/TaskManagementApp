namespace Persistence.Repositories
{
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .Include(p => p.ProjectTasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Project?> CreateAsync(Project project, Guid userId)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Add creator to ProjectUser
            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = Domain.Enums.ProjectRole.Owner
            };
            _context.ProjectUsers.Add(projectUser);
            await _context.SaveChangesAsync();

            return project;
        }
    }
}
