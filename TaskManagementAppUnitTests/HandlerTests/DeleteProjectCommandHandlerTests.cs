using Application.Features.Commands.Projects.DeleteProject;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IProjectAuthorizationService> _authServiceMock = new();
        private readonly Mock<ILogger<DeleteProjectCommandHandler>> _loggerMock = new();
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {
            _handler = new DeleteProjectCommandHandler(_projectRepoMock.Object, _authServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task Handle_UserIdIsEmpty_ThrowsUnauthorizedException()
        {
            // Arrange
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.Empty);

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.UnauthorizedException>()
                .WithMessage("No user ID provided. User must be authenticated.");
        }

        [Fact]
        public async Task Handle_ProjectDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Project with ID * not found.");
        }


        [Fact]
        public async Task Handle_DeleteFails_ThrowsBadRequestException()
        {
            // Arrange
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = Guid.NewGuid(), ProjectName = "Test" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _projectRepoMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Failed to delete the project. Please try again.");
        }


        [Fact]
        public async Task Handle_SuccessfulDelete_ReturnsTrue()
        {
            // Arrange
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project { Id = Guid.NewGuid(), ProjectName = "Test" });
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _projectRepoMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
