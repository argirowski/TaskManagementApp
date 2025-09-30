using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(Guid id);
        Task<bool> DeleteProjectAsync(Guid id);
        Task<Project?> CreateProjectAsync(Project project, Guid userId);
        Task<bool> UpdateProjectAsync(Project project);
        Task<ProjectRole?> GetUserRoleAsync(Guid projectId, Guid userId);
        Task<bool> ProjectExistsByNameAsync(string projectName);
    }
}
