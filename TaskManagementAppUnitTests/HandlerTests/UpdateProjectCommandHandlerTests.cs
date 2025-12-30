using Application.DTOs;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class UpdateProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IProjectAuthorizationService> _authServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<UpdateProjectCommandHandler>> _loggerMock = new();
        private readonly UpdateProjectCommandHandler _handler;

        public UpdateProjectCommandHandlerTests()
        {
            _handler = new UpdateProjectCommandHandler(_projectRepoMock.Object, _authServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ProjectDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            var command = new UpdateProjectCommand(Guid.NewGuid(), new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.NotFoundException>()
                .WithMessage("Project with ID * not found.");
        }

        [Fact]
        public async Task Handle_UpdateFails_ThrowsBadRequestException()
        {
            // Arrange
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "Test" };
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map(It.IsAny<CreateProjectDTO>(), project));
            _projectRepoMock.Setup(x => x.UpdateProjectAsync(project)).ReturnsAsync(false);
            var command = new UpdateProjectCommand(project.Id, new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Failed to update the project. Please try again.");
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsTrue()
        {
            // Arrange
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "Test" };
            _projectRepoMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            _authServiceMock.Setup(x => x.IsUserOwnerAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map(It.IsAny<CreateProjectDTO>(), project));
            _projectRepoMock.Setup(x => x.UpdateProjectAsync(project)).ReturnsAsync(true);
            var command = new UpdateProjectCommand(project.Id, new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
