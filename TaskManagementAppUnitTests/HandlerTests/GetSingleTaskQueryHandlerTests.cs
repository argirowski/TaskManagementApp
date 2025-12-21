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
        private readonly Mock<Domain.Interfaces.IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetSingleTaskQueryHandler _handler;

        public GetSingleTaskQueryHandlerTests()
        {
            _handler = new GetSingleTaskQueryHandler(_taskRepoMock.Object, _projectRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExist_ReturnsNull()
        {
            var projectId = Guid.NewGuid();
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ProjectTask?)null);
            var query = new GetSingleTaskQuery(projectId, Guid.NewGuid());
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Task with ID * not found.");
        }

        [Fact]
        public async Task Handle_TaskExists_ReturnsMappedDto()
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var task = new ProjectTask { Id = taskId, ProjectTaskTitle = "Task1", ProjectTaskDescription = "Desc", ProjectId = projectId };
            var dto = new TaskDTO { ProjectTaskTitle = "Task1", ProjectTaskDescription = "Desc" };
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = projectId, ProjectName = "Test Project" });
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
