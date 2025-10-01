using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using FluentAssertions;
using Persistence;

namespace TaskManagementAppRepositoryTests.Repositories
{
    public class ProjectRepositoryTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateProjectAsync_SavesProjectAndOwner()
        {
            using var context = CreateDbContext();
            var repo = new ProjectRepository(context);
            var userId = Guid.NewGuid();
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "Test", ProjectDescription = "Desc" };

            var result = await repo.CreateProjectAsync(project, userId);

            result.Should().NotBeNull();
            context.Projects.Count().Should().Be(1);
            context.ProjectUsers.Count().Should().Be(1);
            var projectUser = context.ProjectUsers.First();
            projectUser.UserId.Should().Be(userId);
            projectUser.ProjectId.Should().Be(project.Id);
            projectUser.Role.Should().Be(ProjectRole.Owner);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsAllProjects()
        {
            using var context = CreateDbContext();
            context.Projects.Add(new Project { Id = Guid.NewGuid(), ProjectName = "P1" });
            context.Projects.Add(new Project { Id = Guid.NewGuid(), ProjectName = "P2" });
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            var result = await repo.GetAllProjectsAsync();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ReturnsProjectWithUsersAndTasks()
        {
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var project = new Project { Id = projectId, ProjectName = "P1" };
            context.Projects.Add(project);
            context.ProjectUsers.Add(new ProjectUser { ProjectId = projectId, UserId = userId, Role = ProjectRole.Owner });
            context.ProjectTasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = projectId, ProjectTaskTitle = "Task1" });
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            var result = await repo.GetProjectByIdAsync(projectId);
            result.Should().NotBeNull();
            result.ProjectUsers.Should().NotBeNull();
            result.ProjectTasks.Should().NotBeNull();
            result.ProjectUsers.Should().HaveCount(1);
            result.ProjectTasks.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteProjectAsync_RemovesProject()
        {
            using var context = CreateDbContext();
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "P1" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            var result = await repo.DeleteProjectAsync(project.Id);
            result.Should().BeTrue();
            context.Projects.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteProjectAsync_ProjectNotFound_ReturnsFalse()
        {
            using var context = CreateDbContext();
            var repo = new ProjectRepository(context);
            var result = await repo.DeleteProjectAsync(Guid.NewGuid());
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateProjectAsync_UpdatesProject()
        {
            using var context = CreateDbContext();
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "P1" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            project.ProjectName = "Updated";
            var result = await repo.UpdateProjectAsync(project);
            result.Should().BeTrue();
            context.Projects.First().ProjectName.Should().Be("Updated");
        }

        [Fact]
        public async Task GetUserRoleAsync_ReturnsRole()
        {
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            context.ProjectUsers.Add(new ProjectUser { ProjectId = projectId, UserId = userId, Role = ProjectRole.Member });
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            var result = await repo.GetUserRoleAsync(projectId, userId);
            result.Should().Be(ProjectRole.Member);
        }

        [Fact]
        public async Task ProjectExistsByNameAsync_ReturnsTrueIfExists()
        {
            using var context = CreateDbContext();
            context.Projects.Add(new Project { Id = Guid.NewGuid(), ProjectName = "P1" });
            await context.SaveChangesAsync();
            var repo = new ProjectRepository(context);

            var result = await repo.ProjectExistsByNameAsync("P1");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProjectExistsByNameAsync_ReturnsFalseIfNotExists()
        {
            using var context = CreateDbContext();
            var repo = new ProjectRepository(context);
            var result = await repo.ProjectExistsByNameAsync("DoesNotExist");
            result.Should().BeFalse();
        }
    }
}
