using Application.DTOs;
using Application.Features.Queries.Tasks.GetAllTasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class GetAllTasksQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock = new();
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllTasksQueryHandler _handler;

        public GetAllTasksQueryHandlerTests()
        {
            _handler = new GetAllTasksQueryHandler(_taskRepoMock.Object, _projectRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_NoTasks_ReturnsEmptyList()
        {
            _taskRepoMock.Setup(x => x.GetTasksByProjectIdAsync(It.IsAny<Guid>())).ReturnsAsync(Enumerable.Empty<ProjectTask>());
            _mapperMock.Setup(x => x.Map<List<TaskDTO>>(It.IsAny<IEnumerable<ProjectTask>>())).Returns(new List<TaskDTO>());
            var result = await _handler.Handle(new GetAllTasksQuery(Guid.NewGuid()), CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_TasksExist_ReturnsMappedList()
        {
            var projectId = Guid.NewGuid();
            var tasks = new List<ProjectTask> { new ProjectTask { Id = Guid.NewGuid(), ProjectTaskTitle = "Task1", ProjectId = projectId } };
            var dtos = new List<TaskDTO> { new TaskDTO { ProjectTaskTitle = "Task1", ProjectTaskDescription = "Desc" } };
            _taskRepoMock.Setup(x => x.GetTasksByProjectIdAsync(projectId)).ReturnsAsync(tasks.AsEnumerable());
            _mapperMock.Setup(x => x.Map<List<TaskDTO>>(It.IsAny<IEnumerable<ProjectTask>>())).Returns(dtos);
            var result = await _handler.Handle(new GetAllTasksQuery(projectId), CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().ProjectTaskTitle.Should().Be("Task1");
        }
    }
}
