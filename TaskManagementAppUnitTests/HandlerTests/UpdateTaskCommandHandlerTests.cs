using Application.DTOs;
using Application.Features.Commands.Tasks.UpdateTask;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<Domain.Interfaces.IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IProjectAuthorizationService> _authServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _handler = new UpdateTaskCommandHandler(_taskRepoMock.Object, _projectRepoMock.Object, _authServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ThrowsNotFoundException()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var command = new UpdateTaskCommand(projectId, Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Task with ID * not found.");
        }

        [Fact]
        public async Task Handle_UpdateFails_ThrowsBadRequestException()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" };
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map(It.IsAny<TaskDTO>(), task));
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(task)).ReturnsAsync(false);
            var command = new UpdateTaskCommand(projectId, Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Failed to update the task. Please try again.");
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsTrue()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" };
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map(It.IsAny<TaskDTO>(), task));
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(task)).ReturnsAsync(true);
            var command = new UpdateTaskCommand(projectId, Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" }, Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeTrue();
        }
    }
}
