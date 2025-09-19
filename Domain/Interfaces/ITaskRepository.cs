using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId);
        Task<ProjectTask?> GetTaskByIdAsync(Guid projectId, Guid taskId);
    }
}
