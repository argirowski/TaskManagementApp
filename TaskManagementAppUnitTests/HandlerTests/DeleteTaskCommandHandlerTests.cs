using Application.Features.Commands.Tasks.DeleteTask;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<Domain.Interfaces.IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IProjectAuthorizationService> _authServiceMock = new();
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _handler = new DeleteTaskCommandHandler(_taskRepoMock.Object, _projectRepoMock.Object, _authServiceMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ThrowsNotFoundException()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var command = new DeleteTaskCommand(projectId, Guid.NewGuid(), Guid.NewGuid());

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Task with ID * not found.");
        }

        [Fact]
        public async Task Handle_DeleteFails_ThrowsBadRequestException()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.DeleteTaskAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeleteTaskCommand(projectId, Guid.NewGuid(), Guid.NewGuid());

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Failed to delete the task. Please try again.");
        }

        [Fact]
        public async Task Handle_SuccessfulDelete_ReturnsTrue()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.DeleteTaskAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var command = new DeleteTaskCommand(projectId, Guid.NewGuid(), Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeTrue();
        }
    }
}
