using Application.DTOs;
using Application.Features.Commands.Tasks.UpdateTask;
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
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _handler = new UpdateTaskCommandHandler(_taskRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ReturnsFalse()
        {
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var command = new UpdateTaskCommand(Guid.NewGuid(), Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_UpdateFails_ReturnsFalse()
        {
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" };
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map(It.IsAny<TaskDTO>(), task));
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(task)).ReturnsAsync(false);
            var command = new UpdateTaskCommand(Guid.NewGuid(), Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsTrue()
        {
            var task = new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task" };
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map(It.IsAny<TaskDTO>(), task));
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(task)).ReturnsAsync(true);
            var command = new UpdateTaskCommand(Guid.NewGuid(), Guid.NewGuid(), new TaskDTO { ProjectTaskTitle = "Task", ProjectTaskDescription = "Desc" });
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeTrue();
        }
    }
}
