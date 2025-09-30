using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .Include(p => p.ProjectTasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Project?> CreateProjectAsync(Project project, Guid userId)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Add creator to ProjectUser
            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = ProjectRole.Owner
            };
            _context.ProjectUsers.Add(projectUser);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectRole?> GetUserRoleAsync(Guid projectId, Guid userId)
        {
            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            return projectUser?.Role;
        }

        public async Task<bool> ProjectExistsByNameAsync(string projectName)
        {
            return await _context.Projects.AnyAsync(p => p.ProjectName == projectName);
        }
    }
}
