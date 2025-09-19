using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<Project?> CreateAsync(Project project, Guid userId);
    }
}
