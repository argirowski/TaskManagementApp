using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using FluentAssertions;
using Persistence;

namespace TaskManagementAppRepositoryTests.Repositories
{
    public class TaskRepositoryTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateTaskAsync_SavesTask()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new TaskRepository(context);
            var projectId = Guid.NewGuid();
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectId = projectId, ProjectTaskTitle = "Task1" };

            // Act
            var result = await repo.CreateTaskAsync(task);

            // Assert
            result.Should().BeTrue();
            context.ProjectTasks.Count().Should().Be(1);
            context.ProjectTasks.First().ProjectTaskTitle.Should().Be("Task1");
        }

        [Fact]
        public async Task GetTasksByProjectIdAsync_ReturnsTasksForProject()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            context.ProjectTasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = projectId, ProjectTaskTitle = "Task1" });
            context.ProjectTasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = Guid.NewGuid(), ProjectTaskTitle = "Task2" });
            await context.SaveChangesAsync();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.GetTasksByProjectIdAsync(projectId);

            // Assert
            result.Should().HaveCount(1);
            result[0].ProjectTaskTitle.Should().Be("Task1");
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            context.ProjectTasks.Add(new ProjectTask { Id = taskId, ProjectId = projectId, ProjectTaskTitle = "Task1" });
            await context.SaveChangesAsync();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.GetTaskByIdAsync(projectId, taskId);

            // Assert
            result.Should().NotBeNull();
            result.ProjectTaskTitle.Should().Be("Task1");
        }

        [Fact]
        public async Task DeleteTaskAsync_RemovesTask()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            context.ProjectTasks.Add(new ProjectTask { Id = taskId, ProjectId = projectId, ProjectTaskTitle = "Task1" });
            await context.SaveChangesAsync();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.DeleteTaskAsync(projectId, taskId);

            // Assert
            result.Should().BeTrue();
            context.ProjectTasks.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteTaskAsync_TaskNotFound_ReturnsFalse()
        {
            // Arrange
            using var context = CreateDbContext();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.DeleteTaskAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesTask()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectId = projectId, ProjectTaskTitle = "Task1" };
            context.ProjectTasks.Add(task);
            await context.SaveChangesAsync();
            var repo = new TaskRepository(context);

            task.ProjectTaskTitle = "Updated";

            // Act
            var result = await repo.UpdateTaskAsync(task);

            // Assert
            result.Should().BeTrue();
            context.ProjectTasks.First().ProjectTaskTitle.Should().Be("Updated");
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrueIfExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            context.ProjectTasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = projectId, ProjectTaskTitle = "Task1" });
            await context.SaveChangesAsync();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.ExistsByNameAsync(projectId, "Task1");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalseIfNotExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var projectId = Guid.NewGuid();
            var repo = new TaskRepository(context);

            // Act
            var result = await repo.ExistsByNameAsync(projectId, "DoesNotExist");

            // Assert
            result.Should().BeFalse();
        }
    }
}
