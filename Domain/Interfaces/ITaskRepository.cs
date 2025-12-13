using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId);
        Task<ProjectTask?> GetTaskByIdAsync(Guid projectId, Guid taskId);
        Task<ProjectTask?> CreateTaskAsync(ProjectTask task);
        Task<bool> DeleteTaskAsync(Guid projectId, Guid taskId);
        Task<bool> UpdateTaskAsync(ProjectTask task);
        Task<bool> ExistsByNameAsync(Guid projectId, string taskTitle);
    }
}
