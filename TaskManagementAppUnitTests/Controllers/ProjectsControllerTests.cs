using System.Security.Claims;
using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Features.Commands.Projects.DeleteProject;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Features.Queries.Projects.GetAllProjects;
using Application.Features.Queries.Projects.GetSingleProject;
using Application.Helpers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TaskManagementAppUnitTests.Controllers;

public class ProjectsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly ProjectsController _controller;

    public ProjectsControllerTests()
    {
        _controller = new ProjectsController(_mediatorMock.Object);
    }

    private void SetUserContext(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }

    #region CreateProject Tests

    [Fact]
    public async Task CreateProject_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserContext(userId);
        var createProjectDTO = new CreateProjectDTO
        {
            ProjectName = "Test Project",
            ProjectDescription = "Test Description"
        };
        var expectedResult = new CreateProjectDTO
        {
            ProjectName = "Test Project",
            ProjectDescription = "Test Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateProject(createProjectDTO);

        // Assert
        result.Should().NotBeNull();
        var statusCodeResult = result.Result as ObjectResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult!.StatusCode.Should().Be(201);
        statusCodeResult.Value.Should().BeEquivalentTo(expectedResult);

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<CreateProjectCommand>(cmd => 
                cmd.Project.ProjectName == createProjectDTO.ProjectName &&
                cmd.Project.ProjectDescription == createProjectDTO.ProjectDescription &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateProject_WithNullUserId_CreatesCommandWithEmptyGuid()
    {
        // Arrange - Set empty user context (no claims), so GetCurrentUserId returns null
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        var createProjectDTO = new CreateProjectDTO
        {
            ProjectName = "Test Project",
            ProjectDescription = "Test Description"
        };
        var expectedResult = new CreateProjectDTO
        {
            ProjectName = "Test Project",
            ProjectDescription = "Test Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateProject(createProjectDTO);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<CreateProjectCommand>(cmd => cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateProject Tests

    [Fact]
    public async Task UpdateProject_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        SetUserContext(userId);
        var editProjectDTO = new CreateProjectDTO
        {
            ProjectName = "Updated Project",
            ProjectDescription = "Updated Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateProject(projectId, editProjectDTO);

        // Assert
        result.Should().NotBeNull();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<UpdateProjectCommand>(cmd => 
                cmd.Id == projectId &&
                cmd.Project.ProjectName == editProjectDTO.ProjectName &&
                cmd.Project.ProjectDescription == editProjectDTO.ProjectDescription &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProject_WithNullUserId_CreatesCommandWithEmptyGuid()
    {
        // Arrange - Set empty user context (no claims), so GetCurrentUserId returns null
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        var projectId = Guid.NewGuid();
        var editProjectDTO = new CreateProjectDTO
        {
            ProjectName = "Updated Project",
            ProjectDescription = "Updated Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateProject(projectId, editProjectDTO);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<UpdateProjectCommand>(cmd => 
                cmd.Id == projectId &&
                cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteProject Tests

    [Fact]
    public async Task DeleteProject_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        SetUserContext(userId);

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProject(projectId);

        // Assert
        result.Should().NotBeNull();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<DeleteProjectCommand>(cmd => 
                cmd.Id == projectId &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProject_WithNullUserId_CreatesCommandWithEmptyGuid()
    {
        // Arrange - Set empty user context (no claims), so GetCurrentUserId returns null
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        var projectId = Guid.NewGuid();

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProject(projectId);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<DeleteProjectCommand>(cmd => 
                cmd.Id == projectId &&
                cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetAllProjects Tests

    [Fact]
    public async Task GetAllProjects_WithValidPagination_CreatesQueryAndSendsToMediator()
    {
        // Arrange
        var paginationParams = new PaginationParams
        {
            PageNumber = 2,
            PageSize = 10
        };
        var expectedResult = new PagedResultDTO<ProjectDTO>
        {
            Items = new List<ProjectDTO>(),
            TotalCount = 0,
            Page = 2,
            PageSize = 10
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllProjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAllProjects(paginationParams);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        // Verify query was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<GetAllProjectsQuery>(query => 
                query.Page == paginationParams.PageNumber &&
                query.PageSize == paginationParams.PageSize),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetSingleProject Tests

    [Fact]
    public async Task GetSingleProject_WithValidId_CreatesQueryAndSendsToMediator()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var expectedResult = new ProjectDetailsDTO
        {
            ProjectName = "Test Project",
            ProjectDescription = "Test Description",
            Users = new List<UserDetailsDTO>(),
            Tasks = new List<TaskDetailsDTO>()
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetSingleProjectQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetSingleProject(projectId);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        // Verify query was created with correct project ID
        _mediatorMock.Verify(x => x.Send(
            It.Is<GetSingleProjectQuery>(query => query.Id == projectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}

