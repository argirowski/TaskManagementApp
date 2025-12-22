using System.Linq;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagementAppIntegrationTests.Infrastructure;
using Application.DTOs;

namespace TaskManagementAppIntegrationTests.Controllers;

public class TasksControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private const string TestUserId = "11111111-1111-1111-1111-111111111111"; // Matches seeded user ID

    public TasksControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region Helper Methods

    private async Task<Guid> CreateTestProjectAsync(string projectName)
    {
        var createPayload = new
        {
            ProjectName = projectName,
            ProjectDescription = "Test project for tasks"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get all projects to find the one we just created
        // Search through all pages since MaxPageSize=10
        var allProjectsList = new List<ProjectDTO>();
        int pageNumber = 1;
        const int pageSize = 10; // Max allowed by PaginationParams
        int totalPages = int.MaxValue;

        while (pageNumber <= totalPages)
        {
            var getAllResponse = await _client.GetAsync($"/api/Projects?pageNumber={pageNumber}&pageSize={pageSize}");
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var pageResponse = await getAllResponse.Content.ReadFromJsonAsync<PagedProjectsResponse>();
            pageResponse.Should().NotBeNull();
            
            if (totalPages == int.MaxValue)
            {
                totalPages = pageResponse!.TotalPages;
            }
            
            allProjectsList.AddRange(pageResponse!.Items);
            
            var createdProject = pageResponse.Items.FirstOrDefault(p => p.ProjectName == projectName);
            if (createdProject != null)
            {
                return createdProject.Id;
            }
            
            pageNumber++;
        }

        // If not found after searching all pages, fail with helpful message
        var allNames = string.Join(", ", allProjectsList.Select(p => $"'{p.ProjectName}'"));
        throw new InvalidOperationException(
            $"Project '{projectName}' not found after creation. " +
            $"Searched {pageNumber - 1} page(s), found {allProjectsList.Count} projects. " +
            $"Project names: {(string.IsNullOrEmpty(allNames) ? "(none)" : allNames)}");
    }

    private async Task<Guid> CreateTestTaskAsync(Guid projectId, string taskTitle)
    {
        var createPayload = new TaskDTO
        {
            ProjectTaskTitle = taskTitle,
            ProjectTaskDescription = "Test task description"
        };

        var createResponse = await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get all tasks to find the one we just created
        var getAllResponse = await _client.GetAsync($"/api/Tasks/project/{projectId}");
        var allTasks = await getAllResponse.Content.ReadFromJsonAsync<List<TaskDTO>>();
        var createdTask = allTasks!.FirstOrDefault(t => t.ProjectTaskTitle == taskTitle);
        createdTask.Should().NotBeNull();

        // Since TaskDTO doesn't have an Id, we need to get the task details
        // Get the project details which includes tasks with IDs
        var projectResponse = await _client.GetAsync($"/api/Projects/{projectId}");
        var projectDetails = await projectResponse.Content.ReadFromJsonAsync<ProjectDetailsDTO>();
        var task = projectDetails!.Tasks.FirstOrDefault(t => t.ProjectTaskTitle == taskTitle);
        task.Should().NotBeNull();

        return task!.Id;
    }

    #endregion

    #region Get All Tasks Tests

    [Fact]
    public async Task GetAllTasks_WithValidProject_ReturnsOk()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"GetAllTasks {Guid.NewGuid()}");

        // Create some tasks
        var task1 = new TaskDTO
        {
            ProjectTaskTitle = "Task 1",
            ProjectTaskDescription = "Description 1"
        };
        var task2 = new TaskDTO
        {
            ProjectTaskTitle = "Task 2",
            ProjectTaskDescription = "Description 2"
        };

        await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", task1);
        await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", task2);

        // Get all tasks
        var response = await _client.GetAsync($"/api/Tasks/project/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();
        tasks.Should().NotBeNull();
        tasks!.Count.Should().BeGreaterThanOrEqualTo(2);
        tasks.Should().Contain(t => t.ProjectTaskTitle == "Task 1");
        tasks.Should().Contain(t => t.ProjectTaskTitle == "Task 2");
    }

    [Fact]
    public async Task GetAllTasks_WithEmptyProject_ReturnsEmptyList()
    {
        // Create a project without tasks
        var projectId = await CreateTestProjectAsync($"Empty {Guid.NewGuid()}");

        // Get all tasks
        var response = await _client.GetAsync($"/api/Tasks/project/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();
        tasks.Should().NotBeNull();
        tasks!.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllTasks_WithNonExistentProject_ReturnsNotFound()
    {
        var nonExistentProjectId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/Tasks/project/{nonExistentProjectId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Get Single Task Tests

    [Fact]
    public async Task GetSingleTask_WithValidTask_ReturnsOk()
    {
        // Create a project and task
        var projectId = await CreateTestProjectAsync($"GetSingleTask {Guid.NewGuid()}");
        var uniqueTitle = $"Task for GetSingle {Guid.NewGuid()}";
        var taskId = await CreateTestTaskAsync(projectId, uniqueTitle);

        // Get the task
        var response = await _client.GetAsync($"/api/Tasks/project/{projectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var task = await response.Content.ReadFromJsonAsync<TaskDTO>();
        task.Should().NotBeNull();
        task!.ProjectTaskTitle.Should().Be(uniqueTitle);
    }

    [Fact]
    public async Task GetSingleTask_WithNonExistentTask_ReturnsNotFound()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"GetSingleNF {Guid.NewGuid()}");
        var nonExistentTaskId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/Tasks/project/{projectId}/task/{nonExistentTaskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSingleTask_WithNonExistentProject_ReturnsNotFound()
    {
        var nonExistentProjectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/Tasks/project/{nonExistentProjectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Create Task Tests

    [Fact]
    public async Task CreateTask_WithValidData_ReturnsCreated()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"CreateTask {Guid.NewGuid()}");

        var payload = new TaskDTO
        {
            ProjectTaskTitle = $"New Task {Guid.NewGuid()}",
            ProjectTaskDescription = "A new test task"
        };

        var response = await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadFromJsonAsync<TaskDTO>();
        json.Should().NotBeNull();
        json!.ProjectTaskTitle.Should().Be(payload.ProjectTaskTitle);
        json.ProjectTaskDescription.Should().Be(payload.ProjectTaskDescription);
    }

    [Fact]
    public async Task CreateTask_WithDuplicateName_ReturnsBadRequest()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"DuplicateTask {Guid.NewGuid()}");
        var taskTitle = $"Duplicate Task {Guid.NewGuid()}";

        // Create first task
        var firstTask = new TaskDTO
        {
            ProjectTaskTitle = taskTitle,
            ProjectTaskDescription = "First task"
        };
        var firstResponse = await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", firstTask);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Try to create another task with the same name
        var secondTask = new TaskDTO
        {
            ProjectTaskTitle = taskTitle, // Duplicate name
            ProjectTaskDescription = "Second task"
        };
        var response = await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", secondTask);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTask_WithEmptyTitle_ReturnsBadRequest()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"EmptyTitle {Guid.NewGuid()}");

        var payload = new TaskDTO
        {
            ProjectTaskTitle = "", // Empty title
            ProjectTaskDescription = "Description"
        };

        var response = await _client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTask_WithNonExistentProject_ReturnsNotFound()
    {
        var nonExistentProjectId = Guid.NewGuid();

        var payload = new TaskDTO
        {
            ProjectTaskTitle = "Task Title",
            ProjectTaskDescription = "Description"
        };

        var response = await _client.PostAsJsonAsync($"/api/Tasks/project/{nonExistentProjectId}", payload);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Update Task Tests

    [Fact]
    public async Task UpdateTask_AsOwner_ReturnsNoContent()
    {
        // Create a project and task
        var projectId = await CreateTestProjectAsync($"UpdateTask {Guid.NewGuid()}");
        var uniqueTitle = $"Task to Update {Guid.NewGuid()}";
        var taskId = await CreateTestTaskAsync(projectId, uniqueTitle);

        // Update the task
        var updatePayload = new TaskDTO
        {
            ProjectTaskTitle = "Updated Task Title",
            ProjectTaskDescription = "Updated description"
        };

        var response = await _client.PutAsJsonAsync($"/api/Tasks/project/{projectId}/task/{taskId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update by getting the task
        var getResponse = await _client.GetAsync($"/api/Tasks/project/{projectId}/task/{taskId}");
        var updatedTask = await getResponse.Content.ReadFromJsonAsync<TaskDTO>();
        updatedTask!.ProjectTaskTitle.Should().Be("Updated Task Title");
        updatedTask.ProjectTaskDescription.Should().Be("Updated description");
    }

    [Fact]
    public async Task UpdateTask_WithNonExistentTask_ReturnsNotFound()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"UpdateNF {Guid.NewGuid()}");
        var nonExistentTaskId = Guid.NewGuid();

        var updatePayload = new TaskDTO
        {
            ProjectTaskTitle = "Updated Title",
            ProjectTaskDescription = "Updated description"
        };

        var response = await _client.PutAsJsonAsync($"/api/Tasks/project/{projectId}/task/{nonExistentTaskId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTask_WithNonExistentProject_ReturnsNotFound()
    {
        var nonExistentProjectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var updatePayload = new TaskDTO
        {
            ProjectTaskTitle = "Updated Title",
            ProjectTaskDescription = "Updated description"
        };

        var response = await _client.PutAsJsonAsync($"/api/Tasks/project/{nonExistentProjectId}/task/{taskId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Delete Task Tests

    [Fact]
    public async Task DeleteTask_AsOwner_ReturnsNoContent()
    {
        // Create a project and task
        var projectId = await CreateTestProjectAsync($"DeleteTask {Guid.NewGuid()}");
        var uniqueTitle = $"Task to Delete {Guid.NewGuid()}";
        var taskId = await CreateTestTaskAsync(projectId, uniqueTitle);

        // Delete the task
        var response = await _client.DeleteAsync($"/api/Tasks/project/{projectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion by trying to get the task
        var getResponse = await _client.GetAsync($"/api/Tasks/project/{projectId}/task/{taskId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTask_WithNonExistentTask_ReturnsNotFound()
    {
        // Create a project
        var projectId = await CreateTestProjectAsync($"DeleteNF {Guid.NewGuid()}");
        var nonExistentTaskId = Guid.NewGuid();

        var response = await _client.DeleteAsync($"/api/Tasks/project/{projectId}/task/{nonExistentTaskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTask_WithNonExistentProject_ReturnsNotFound()
    {
        var nonExistentProjectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var response = await _client.DeleteAsync($"/api/Tasks/project/{nonExistentProjectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Response DTOs

    private sealed record PagedProjectsResponse
    {
        public List<ProjectDTO> Items { get; init; } = new();
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }

    private sealed record ProjectDTO
    {
        public Guid Id { get; init; }
        public string ProjectName { get; init; } = string.Empty;
        public string ProjectDescription { get; init; } = string.Empty;
    }


    #endregion
}

