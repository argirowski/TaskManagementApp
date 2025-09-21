using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId);
        Task<ProjectTask?> GetTaskByIdAsync(Guid projectId, Guid taskId);
        Task<bool> CreateTaskAsync(ProjectTask task);
        Task<bool> DeleteTaskAsync(Guid projectId, Guid taskId);
        Task<bool> UpdateTaskAsync(ProjectTask task);
    }
}
