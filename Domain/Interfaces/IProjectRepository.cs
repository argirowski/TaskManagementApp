using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<Project?> CreateAsync(Project project, Guid userId);
        Task<bool> UpdateAsync(Project project);
        Task<ProjectRole?> GetUserRoleAsync(Guid projectId, Guid userId);
    }
}
