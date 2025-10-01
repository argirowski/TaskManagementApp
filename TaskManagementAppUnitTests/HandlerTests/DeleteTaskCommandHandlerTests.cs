using Application.Features.Commands.Tasks.DeleteTask;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _handler = new DeleteTaskCommandHandler(_taskRepoMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ReturnsFalse()
        {
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_DeleteFails_ReturnsFalse()
        {
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.DeleteTaskAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_SuccessfulDelete_ReturnsTrue()
        {
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" });
            _taskRepoMock.Setup(x => x.DeleteTaskAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeTrue();
        }
    }
}
