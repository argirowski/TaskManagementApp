using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _handler = new CreateTaskCommandHandler(_taskRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_DuplicateTaskName_ReturnsNull()
        {
            // Arrange
            _taskRepoMock.Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);
            var command = new CreateTaskCommand(Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            _taskRepoMock.Setup(x => x.ExistsByNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<ProjectTask>(It.IsAny<TaskDTO>())).Returns(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.CreateTaskAsync(It.IsAny<ProjectTask>())).ReturnsAsync((ProjectTask?)null);
            var command = new CreateTaskCommand(Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_SuccessfulCreation_ReturnsDto()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task", ProjectId = projectId };
            var dto = new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" };

            _taskRepoMock.Setup(x => x.ExistsByNameAsync(projectId, "Task")).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<ProjectTask>(It.IsAny<TaskDTO>())).Returns(task);
            _taskRepoMock.Setup(x => x.CreateTaskAsync(task)).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map<TaskDTO>(task)).Returns(dto);
            var command = new CreateTaskCommand(projectId, new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectTaskTitle.Should().Be("Task");
            result.ProjectTaskDescription.Should().Be("Desc");
        }
    }
}
