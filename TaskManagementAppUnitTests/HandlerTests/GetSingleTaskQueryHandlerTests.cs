using Application.DTOs;
using Application.Features.Queries.Tasks.GetSingleTask;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class GetSingleTaskQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetSingleTaskQueryHandler _handler;

        public GetSingleTaskQueryHandlerTests()
        {
            _handler = new GetSingleTaskQueryHandler(_taskRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ReturnsNull()
        {
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var query = new GetSingleTaskQuery(Guid.NewGuid(), Guid.NewGuid());
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_TaskExists_ReturnsMappedDto()
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var task = new ProjectTask { Id = taskId, ProjectTaskTitle = "Task1", ProjectTaskDescription = "Desc", ProjectId = projectId };
            var dto = new TaskDTO { ProjectTaskTitle = "Task1", ProjectTaskDescription = "Desc" };
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(projectId, taskId)).ReturnsAsync(task);
            _mapperMock.Setup(x => x.Map<TaskDTO>(task)).Returns(dto);
            var query = new GetSingleTaskQuery(projectId, taskId);
            var result = await _handler.Handle(query, CancellationToken.None);
            result.Should().NotBeNull();
            result.ProjectTaskTitle.Should().Be("Task1");
            result.ProjectTaskDescription.Should().Be("Desc");
        }
    }
}
