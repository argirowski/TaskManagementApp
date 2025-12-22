using System.Linq;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagementAppIntegrationTests.Infrastructure;
using Application.DTOs;

namespace TaskManagementAppIntegrationTests.Controllers;

public class AuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private const string FirstUserId = TestAuthHandler.DefaultUserId;
    private const string SecondUserId = TestAuthHandler.SecondUserId;

    public AuthorizationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    #region Helper Methods

    private HttpClient CreateClientForUser(string userId)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        // Set the test user ID header
        client.DefaultRequestHeaders.Add("X-Test-User-Id", userId);
        
        return client;
    }

    private async Task<Guid> CreateProjectForUserAsync(HttpClient client, string projectName)
    {
        var createPayload = new
        {
            ProjectName = projectName,
            ProjectDescription = "Test project"
        };

        var createResponse = await client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, 
            $"Project creation failed. Status: {createResponse.StatusCode}, Content: {await createResponse.Content.ReadAsStringAsync()}");

        // Small delay to ensure database consistency
        await Task.Delay(50);

        // Search through pages to find the project
        // Note: PaginationParams has MaxPageSize = 10, so we need to iterate through pages
        int pageNumber = 1;
        const int pageSize = 10; // Max allowed by PaginationParams
        int totalPages = int.MaxValue;
        var allProjectsList = new List<ProjectDTO>();

        while (pageNumber <= totalPages)
        {
            var getAllResponse = await client.GetAsync($"/api/Projects?pageNumber={pageNumber}&pageSize={pageSize}");
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var allProjects = await getAllResponse.Content.ReadFromJsonAsync<PagedProjectsResponse>();
            allProjects.Should().NotBeNull("Projects response should not be null");
            
            // Update total pages from response (should be same for all pages, but get it from first)
            if (totalPages == int.MaxValue)
            {
                totalPages = allProjects!.TotalPages;
            }
            
            // Collect all projects for debugging
            allProjectsList.AddRange(allProjects!.Items);
            
            var createdProject = allProjects.Items.FirstOrDefault(p => p.ProjectName == projectName);
            if (createdProject != null)
            {
                return createdProject.Id;
            }
            
            pageNumber++;
            
            // Safety check to prevent infinite loops
            if (pageNumber > 100)
            {
                break;
            }
        }

        // If we've searched all pages and still not found, provide detailed error
        var allNames = string.Join(", ", allProjectsList.Select(p => $"'{p.ProjectName}'"));
        throw new InvalidOperationException(
            $"Project '{projectName}' not found after creation. " +
            $"Searched {pageNumber - 1} page(s) (total pages: {totalPages}), " +
            $"found {allProjectsList.Count} projects. " +
            $"Project names found: {(string.IsNullOrEmpty(allNames) ? "(none)" : allNames)}");
    }

    private async Task<Guid> CreateTaskForProjectAsync(HttpClient client, Guid projectId, string taskTitle)
    {
        var createPayload = new TaskDTO
        {
            ProjectTaskTitle = taskTitle,
            ProjectTaskDescription = "Test task description"
        };

        var createResponse = await client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get project details to find the task ID
        var projectResponse = await client.GetAsync($"/api/Projects/{projectId}");
        var projectDetails = await projectResponse.Content.ReadFromJsonAsync<ProjectDetailsDTO>();
        var task = projectDetails!.Tasks.FirstOrDefault(t => t.ProjectTaskTitle == taskTitle);
        task.Should().NotBeNull();

        return task!.Id;
    }

    #endregion

    #region Projects Authorization Tests

    [Fact]
    public async Task UpdateProject_AsNonOwner_ReturnsForbidden()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 2 tries to update User 1's project
        var user2Client = CreateClientForUser(SecondUserId);
        var updatePayload = new
        {
            ProjectName = "Hacked Project Name",
            ProjectDescription = "Hacked description"
        };

        var response = await user2Client.PutAsJsonAsync($"/api/Projects/{projectId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteProject_AsNonOwner_ReturnsForbidden()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 2 tries to delete User 1's project
        var user2Client = CreateClientForUser(SecondUserId);
        var response = await user2Client.DeleteAsync($"/api/Projects/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateProject_AsOwner_ReturnsNoContent()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 1 updates their own project
        var updatePayload = new
        {
            ProjectName = "Updated Project Name",
            ProjectDescription = "Updated description"
        };

        var response = await user1Client.PutAsJsonAsync($"/api/Projects/{projectId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProject_AsOwner_ReturnsNoContent()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 1 deletes their own project
        var response = await user1Client.DeleteAsync($"/api/Projects/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetSingleProject_AsNonOwner_ReturnsOk()
    {
        // Note: Currently, GetSingleProject doesn't check ownership - any authenticated user can view any project
        // This test verifies the current behavior (which may or may not be desired)
        
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 2 can view User 1's project (current behavior)
        var user2Client = CreateClientForUser(SecondUserId);
        var response = await user2Client.GetAsync($"/api/Projects/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllProjects_ReturnsAllProjects()
    {
        // Note: Currently, GetAllProjects doesn't filter by user - any authenticated user can view all projects
        // This test verifies the current behavior (which may or may not be desired)
        
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var user1ProjectId = await CreateProjectForUserAsync(user1Client, $"User1 {Guid.NewGuid()}");

        // User 2 creates a project
        var user2Client = CreateClientForUser(SecondUserId);
        var user2ProjectId = await CreateProjectForUserAsync(user2Client, $"User2 {Guid.NewGuid()}");

        // Helper to get all projects across all pages
        async Task<List<ProjectDTO>> GetAllProjectsForUser(HttpClient client)
        {
            var allProjects = new List<ProjectDTO>();
            int pageNumber = 1;
            const int pageSize = 10; // Max allowed by PaginationParams
            int totalPages = int.MaxValue;

            while (pageNumber <= totalPages)
            {
                var response = await client.GetAsync($"/api/Projects?pageNumber={pageNumber}&pageSize={pageSize}");
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var pagedResult = await response.Content.ReadFromJsonAsync<PagedProjectsResponse>();
                pagedResult.Should().NotBeNull();
                
                if (totalPages == int.MaxValue)
                {
                    totalPages = pagedResult!.TotalPages;
                }
                
                allProjects.AddRange(pagedResult!.Items);
                pageNumber++;
            }

            return allProjects;
        }

        // User 1 can see all projects including User 2's project (current behavior)
        var user1Projects = await GetAllProjectsForUser(user1Client);
        user1Projects.Should().Contain(p => p.Id == user1ProjectId);
        user1Projects.Should().Contain(p => p.Id == user2ProjectId); // Current behavior: can see all projects

        // User 2 can see all projects including User 1's project (current behavior)
        var user2Projects = await GetAllProjectsForUser(user2Client);
        user2Projects.Should().Contain(p => p.Id == user2ProjectId);
        user2Projects.Should().Contain(p => p.Id == user1ProjectId); // Current behavior: can see all projects
    }

    #endregion

    #region Tasks Authorization Tests

    [Fact]
    public async Task CreateTask_AsNonOwner_ReturnsForbidden()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 2 tries to create a task in User 1's project
        var user2Client = CreateClientForUser(SecondUserId);
        var taskPayload = new TaskDTO
        {
            ProjectTaskTitle = "Unauthorized Task",
            ProjectTaskDescription = "Should not be created"
        };

        var response = await user2Client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", taskPayload);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateTask_AsNonOwner_ReturnsForbidden()
    {
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        var taskId = await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 2 tries to update the task
        var user2Client = CreateClientForUser(SecondUserId);
        var updatePayload = new TaskDTO
        {
            ProjectTaskTitle = "Hacked Task Title",
            ProjectTaskDescription = "Hacked description"
        };

        var response = await user2Client.PutAsJsonAsync($"/api/Tasks/project/{projectId}/task/{taskId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteTask_AsNonOwner_ReturnsForbidden()
    {
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        var taskId = await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 2 tries to delete the task
        var user2Client = CreateClientForUser(SecondUserId);
        var response = await user2Client.DeleteAsync($"/api/Tasks/project/{projectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateTask_AsOwner_ReturnsCreated()
    {
        // User 1 creates a project
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");

        // User 1 creates a task in their own project
        var taskPayload = new TaskDTO
        {
            ProjectTaskTitle = $"Authorized Task {Guid.NewGuid()}",
            ProjectTaskDescription = "Should be created"
        };

        var response = await user1Client.PostAsJsonAsync($"/api/Tasks/project/{projectId}", taskPayload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateTask_AsOwner_ReturnsNoContent()
    {
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        var taskId = await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 1 updates their own task
        var updatePayload = new TaskDTO
        {
            ProjectTaskTitle = "Updated Task Title",
            ProjectTaskDescription = "Updated description"
        };

        var response = await user1Client.PutAsJsonAsync($"/api/Tasks/project/{projectId}/task/{taskId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTask_AsOwner_ReturnsNoContent()
    {
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        var taskId = await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 1 deletes their own task
        var response = await user1Client.DeleteAsync($"/api/Tasks/project/{projectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetAllTasks_AsNonOwner_ReturnsOk()
    {
        // Note: Currently, GetAllTasks doesn't check ownership - any authenticated user can view tasks
        // This test verifies the current behavior (which may or may not be desired)
        
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 2 can view tasks in User 1's project (current behavior)
        var user2Client = CreateClientForUser(SecondUserId);
        var response = await user2Client.GetAsync($"/api/Tasks/project/{projectId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSingleTask_AsNonOwner_ReturnsOk()
    {
        // Note: Currently, GetSingleTask doesn't check ownership - any authenticated user can view any task
        // This test verifies the current behavior (which may or may not be desired)
        
        // User 1 creates a project and task
        var user1Client = CreateClientForUser(FirstUserId);
        var projectId = await CreateProjectForUserAsync(user1Client, $"Project {Guid.NewGuid()}");
        var taskId = await CreateTaskForProjectAsync(user1Client, projectId, $"Task {Guid.NewGuid()}");

        // User 2 can view User 1's task (current behavior)
        var user2Client = CreateClientForUser(SecondUserId);
        var response = await user2Client.GetAsync($"/api/Tasks/project/{projectId}/task/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
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

