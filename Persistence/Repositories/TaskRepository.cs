using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId)
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
        public async Task<ProjectTask?> CreateTaskAsync(ProjectTask task)
        {
            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task<bool> DeleteTaskAsync(Guid projectId, Guid taskId)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);
            if (task == null)
            {
                return false;
            }

            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateTaskAsync(ProjectTask task)
        {
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsByNameAsync(Guid projectId, string taskTitle)
        {
            return await _context.ProjectTasks
                .AnyAsync(t => t.ProjectId == projectId && t.ProjectTaskTitle == taskTitle);
        }
    }
}
