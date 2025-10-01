using Application.Features.Commands.Projects.DeleteProject;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {
            _handler = new DeleteProjectCommandHandler(_projectRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ProjectDoesNotExist_ReturnsFalse()
        {
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            var command = new DeleteProjectCommand(Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_DeleteFails_ReturnsFalse()
        {
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = Guid.NewGuid(), ProjectName = "Test" });
            _projectRepoMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeleteProjectCommand(Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_SuccessfulDelete_ReturnsTrue()
        {
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = Guid.NewGuid(), ProjectName = "Test" });
            _projectRepoMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            var command = new DeleteProjectCommand(Guid.NewGuid());
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeTrue();
        }
    }
}
