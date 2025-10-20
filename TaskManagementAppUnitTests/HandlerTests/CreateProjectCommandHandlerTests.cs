using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {
            _handler = new CreateProjectCommandHandler(_projectRepoMock.Object, _userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            var command = new CreateProjectCommand(new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ProjectNameExists_ReturnsNull()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User { Id = Guid.NewGuid(), UserName = "User", UserEmail = "user@mail.com" });
            _projectRepoMock.Setup(x => x.ProjectExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(true);
            var command = new CreateProjectCommand(new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User { Id = Guid.NewGuid(), UserName = "User", UserEmail = "user@mail.com" });
            _projectRepoMock.Setup(x => x.ProjectExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Project>(It.IsAny<CreateProjectDTO>())).Returns(new Project { Id = Guid.NewGuid(), ProjectName = "Test" });
            _projectRepoMock.Setup(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            var command = new CreateProjectCommand(new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_SuccessfulCreation_ReturnsDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "Test" };
            var createdProject = new Project { Id = Guid.NewGuid(), ProjectName = "Test" };
            var dto = new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" };

            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User { Id = userId, UserName = "User", UserEmail = "user@mail.com" });
            _projectRepoMock.Setup(x => x.ProjectExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Project>(It.IsAny<CreateProjectDTO>())).Returns(project);
            _projectRepoMock.Setup(x => x.CreateProjectAsync(It.IsAny<Project>(), userId)).ReturnsAsync(createdProject);
            _mapperMock.Setup(x => x.Map<CreateProjectDTO>(createdProject)).Returns(dto);
            var command = new CreateProjectCommand(new CreateProjectDTO { ProjectName = "Test", ProjectDescription = "Desc" }, userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectName.Should().Be("Test");
            result.ProjectDescription.Should().Be("Desc");
        }
    }
}
