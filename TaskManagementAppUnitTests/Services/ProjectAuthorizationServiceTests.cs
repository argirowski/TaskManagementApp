using Application.DTOs;
using Application.Services;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.Services;

public class ProjectAuthorizationServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepoMock = new();
    private readonly ProjectAuthorizationService _service;

    public ProjectAuthorizationServiceTests()
    {
        _service = new ProjectAuthorizationService(_projectRepoMock.Object);
    }

    #region IsUserOwnerAsync Tests

    [Fact]
    public async Task IsUserOwnerAsync_UserIsOwner_ReturnsTrue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync(ProjectRole.Owner);

        // Act
        var result = await _service.IsUserOwnerAsync(projectId, userId);

        // Assert
        result.Should().BeTrue();
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task IsUserOwnerAsync_UserIsMember_ReturnsFalse()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync(ProjectRole.Member);

        // Act
        var result = await _service.IsUserOwnerAsync(projectId, userId);

        // Assert
        result.Should().BeFalse();
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task IsUserOwnerAsync_UserHasNoRole_ReturnsFalse()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync((ProjectRole?)null);

        // Act
        var result = await _service.IsUserOwnerAsync(projectId, userId);

        // Assert
        result.Should().BeFalse();
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    #endregion

    #region ValidateProjectOwnerAsync Tests

    [Fact]
    public async Task ValidateProjectOwnerAsync_UserIsOwner_ReturnsSuccess()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync(ProjectRole.Owner);

        // Act
        var result = await _service.ValidateProjectOwnerAsync(projectId, userId);

        // Assert
        result.Should().NotBeNull();
        result.IsAuthorized.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        // Verify it's the same instance type as Success() creates
        result.Should().BeEquivalentTo(AuthorizationResultDTO.Success());
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task ValidateProjectOwnerAsync_UserIsMember_ReturnsFailure()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync(ProjectRole.Member);

        // Act
        var result = await _service.ValidateProjectOwnerAsync(projectId, userId);

        // Assert
        result.Should().NotBeNull();
        result.IsAuthorized.Should().BeFalse();
        result.ErrorMessage.Should().Be("You don't have permission for this operation");
        // Verify it matches the Failure() method output
        var expectedFailure = AuthorizationResultDTO.Failure("You don't have permission for this operation");
        result.Should().BeEquivalentTo(expectedFailure);
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task ValidateProjectOwnerAsync_UserHasNoRole_ReturnsFailure()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync((ProjectRole?)null);

        // Act
        var result = await _service.ValidateProjectOwnerAsync(projectId, userId);

        // Assert
        result.Should().NotBeNull();
        result.IsAuthorized.Should().BeFalse();
        result.ErrorMessage.Should().Be("You don't have permission for this operation");
        // Verify it matches the Failure() method output
        var expectedFailure = AuthorizationResultDTO.Failure("You don't have permission for this operation");
        result.Should().BeEquivalentTo(expectedFailure);
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task ValidateProjectOwnerAsync_CallsIsUserOwnerAsync()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId))
            .ReturnsAsync(ProjectRole.Owner);

        // Act
        var result = await _service.ValidateProjectOwnerAsync(projectId, userId);

        // Assert
        result.Should().NotBeNull();
        // Verify that GetUserRoleAsync was called (which IsUserOwnerAsync calls internally)
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task IsUserOwnerAsync_WithDifferentProjectIds_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId1, userId))
            .ReturnsAsync(ProjectRole.Owner);
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId2, userId))
            .ReturnsAsync(ProjectRole.Member);

        // Act
        var result1 = await _service.IsUserOwnerAsync(projectId1, userId);
        var result2 = await _service.IsUserOwnerAsync(projectId2, userId);

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeFalse();
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId1, userId), Times.Once);
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId2, userId), Times.Once);
    }

    [Fact]
    public async Task IsUserOwnerAsync_WithDifferentUserIds_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId1))
            .ReturnsAsync(ProjectRole.Owner);
        _projectRepoMock.Setup(x => x.GetUserRoleAsync(projectId, userId2))
            .ReturnsAsync((ProjectRole?)null);

        // Act
        var result1 = await _service.IsUserOwnerAsync(projectId, userId1);
        var result2 = await _service.IsUserOwnerAsync(projectId, userId2);

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeFalse();
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId1), Times.Once);
        _projectRepoMock.Verify(x => x.GetUserRoleAsync(projectId, userId2), Times.Once);
    }

    #endregion
}

