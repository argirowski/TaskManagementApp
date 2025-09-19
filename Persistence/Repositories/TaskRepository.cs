namespace Persistence.Repositories
{
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }
        public async Task<ProjectTask?> GetTaskByIdAsync(Guid projectId, Guid taskId)
        {
            return await _context.ProjectTasks
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);
        }
    }
}
