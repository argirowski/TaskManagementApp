using System.Security.Claims;
using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using Application.Features.Commands.Tasks.DeleteTask;
using Application.Features.Commands.Tasks.UpdateTask;
using Application.Features.Queries.Tasks.GetAllTasks;
using Application.Features.Queries.Tasks.GetSingleTask;
using Application.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TaskManagementAppUnitTests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IProjectAuthorizationService> _authorizationServiceMock = new();
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _controller = new TasksController(_mediatorMock.Object, _authorizationServiceMock.Object);
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

    #region CreateTask Tests

    [Fact]
    public async Task CreateTask_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        SetUserContext(userId);
        var taskDTO = new TaskDTO
        {
            ProjectTaskTitle = "Test Task",
            ProjectTaskDescription = "Test Description"
        };
        var expectedResult = new TaskDTO
        {
            ProjectTaskTitle = "Test Task",
            ProjectTaskDescription = "Test Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateTask(projectId, taskDTO);

        // Assert
        result.Should().NotBeNull();
        var statusCodeResult = result.Result as ObjectResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult!.StatusCode.Should().Be(201);
        statusCodeResult.Value.Should().BeEquivalentTo(expectedResult);

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<CreateTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.Task.ProjectTaskTitle == taskDTO.ProjectTaskTitle &&
                cmd.Task.ProjectTaskDescription == taskDTO.ProjectTaskDescription &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTask_WithNullUserId_CreatesCommandWithEmptyGuid()
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
        var taskDTO = new TaskDTO
        {
            ProjectTaskTitle = "Test Task",
            ProjectTaskDescription = "Test Description"
        };
        var expectedResult = new TaskDTO
        {
            ProjectTaskTitle = "Test Task",
            ProjectTaskDescription = "Test Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateTask(projectId, taskDTO);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<CreateTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateTask Tests

    [Fact]
    public async Task UpdateTask_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        SetUserContext(userId);
        var taskDTO = new TaskDTO
        {
            ProjectTaskTitle = "Updated Task",
            ProjectTaskDescription = "Updated Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateTask(projectId, taskId, taskDTO);

        // Assert
        result.Should().NotBeNull();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<UpdateTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.TaskId == taskId &&
                cmd.Task.ProjectTaskTitle == taskDTO.ProjectTaskTitle &&
                cmd.Task.ProjectTaskDescription == taskDTO.ProjectTaskDescription &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTask_WithNullUserId_CreatesCommandWithEmptyGuid()
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
        var taskId = Guid.NewGuid();
        var taskDTO = new TaskDTO
        {
            ProjectTaskTitle = "Updated Task",
            ProjectTaskDescription = "Updated Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateTask(projectId, taskId, taskDTO);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<UpdateTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.TaskId == taskId &&
                cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteTask Tests

    [Fact]
    public async Task DeleteTask_WithValidData_CreatesCommandAndSendsToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        SetUserContext(userId);

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTask(projectId, taskId);

        // Assert
        result.Should().NotBeNull();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();

        // Verify command was created with correct parameters
        _mediatorMock.Verify(x => x.Send(
            It.Is<DeleteTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.TaskId == taskId &&
                cmd.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTask_WithNullUserId_CreatesCommandWithEmptyGuid()
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
        var taskId = Guid.NewGuid();

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTask(projectId, taskId);

        // Assert
        result.Should().NotBeNull();
        
        // Verify command was created with Guid.Empty when userId is null
        _mediatorMock.Verify(x => x.Send(
            It.Is<DeleteTaskCommand>(cmd => 
                cmd.ProjectId == projectId &&
                cmd.TaskId == taskId &&
                cmd.UserId == Guid.Empty),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetAllTasksForProject Tests

    [Fact]
    public async Task GetAllTasksForProject_WithValidProjectId_CreatesQueryAndSendsToMediator()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var expectedResult = new List<TaskDTO>
        {
            new TaskDTO
            {
                ProjectTaskTitle = "Task 1",
                ProjectTaskDescription = "Description 1"
            },
            new TaskDTO
            {
                ProjectTaskTitle = "Task 2",
                ProjectTaskDescription = "Description 2"
            }
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAllTasksForProject(projectId);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        // Verify query was created with correct project ID
        _mediatorMock.Verify(x => x.Send(
            It.Is<GetAllTasksQuery>(query => query.ProjectId == projectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetSingleTaskForProject Tests

    [Fact]
    public async Task GetSingleTaskForProject_WithValidIds_CreatesQueryAndSendsToMediator()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var expectedResult = new TaskDTO
        {
            ProjectTaskTitle = "Test Task",
            ProjectTaskDescription = "Test Description"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetSingleTaskQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetSingleTaskForProject(projectId, taskId);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        // Verify query was created with correct project and task IDs
        _mediatorMock.Verify(x => x.Send(
            It.Is<GetSingleTaskQuery>(query => 
                query.ProjectId == projectId &&
                query.TaskId == taskId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}

