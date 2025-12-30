using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IProjectAuthorizationService> _authServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock = new();
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _handler = new CreateTaskCommandHandler(_taskRepoMock.Object, _projectRepoMock.Object, _mapperMock.Object, _authServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DuplicateTaskName_ThrowsBadRequestException()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);
            var command = new CreateTaskCommand(projectId, new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("A task with the name 'Task' already exists.");
        }

        [Fact]
        public async Task Handle_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<ProjectTask>(It.IsAny<TaskDTO>())).Returns(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.CreateTaskAsync(It.IsAny<ProjectTask>())).ReturnsAsync((ProjectTask?)null);
            var command = new CreateTaskCommand(projectId, new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Failed to create the task. Please try again.");
        }

        [Fact]
        public async Task Handle_SuccessfulCreation_ReturnsDto()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task", ProjectId = projectId };
            var dto = new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" };

            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.ExistsByNameAsync(projectId, "Task")).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<ProjectTask>(It.IsAny<TaskDTO>())).Returns(task);
            _taskRepoMock.Setup(x => x.CreateTaskAsync(task)).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map<TaskDTO>(task)).Returns(dto);
            var command = new CreateTaskCommand(projectId, new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectTaskTitle.Should().Be("Task");
            result.ProjectTaskDescription.Should().Be("Desc");
        }
    }
}
