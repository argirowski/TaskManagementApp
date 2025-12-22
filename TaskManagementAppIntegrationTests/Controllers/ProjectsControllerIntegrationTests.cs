using System.Linq;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagementAppIntegrationTests.Infrastructure;

namespace TaskManagementAppIntegrationTests.Controllers;

public class ProjectsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region Get All Projects Tests

    [Fact]
    public async Task GetAllProjects_WithAuthentication_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Projects?pageNumber=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<PagedProjectsResponse>();
        json.Should().NotBeNull();
        json!.Items.Should().NotBeNull();
        json.TotalCount.Should().BeGreaterThanOrEqualTo(0);
        json.Page.Should().Be(1);
        json.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetAllProjects_WithPagination_ReturnsCorrectPage()
    {
        var response = await _client.GetAsync("/api/Projects?pageNumber=1&pageSize=5");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<PagedProjectsResponse>();
        json.Should().NotBeNull();
        json!.PageSize.Should().Be(5);
        json.Items.Should().HaveCountLessThanOrEqualTo(5);
    }

    #endregion

    #region Get Single Project Tests

    [Fact]
    public async Task GetSingleProject_WithValidId_ReturnsOk()
    {
        // First, create a project
        var uniqueName = $"Test Project for Get {Guid.NewGuid()}";
        var createPayload = new
        {
            ProjectName = uniqueName,
            ProjectDescription = "Test description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get all projects to find the one we just created
        var getAllResponse = await _client.GetAsync("/api/Projects?pageNumber=1&pageSize=100");
        var allProjects = await getAllResponse.Content.ReadFromJsonAsync<PagedProjectsResponse>();
        var createdProject = allProjects!.Items.FirstOrDefault(p => p.ProjectName == uniqueName);
        createdProject.Should().NotBeNull();

        // Now get the project by ID
        var response = await _client.GetAsync($"/api/Projects/{createdProject!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<ProjectDetailsResponse>();
        json.Should().NotBeNull();
        json!.ProjectName.Should().Be(uniqueName);
        json.ProjectDescription.Should().Be("Test description");
    }

    [Fact]
    public async Task GetSingleProject_WithNonExistentId_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/Projects/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Create Project Tests

    [Fact]
    public async Task CreateProject_WithValidData_ReturnsCreated()
    {
        var payload = new
        {
            ProjectName = $"New Project {Guid.NewGuid()}",
            ProjectDescription = "A new test project"
        };

        var response = await _client.PostAsJsonAsync("/api/Projects", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadFromJsonAsync<CreateProjectResponse>();
        json.Should().NotBeNull();
        json!.ProjectName.Should().Be(payload.ProjectName);
        json.ProjectDescription.Should().Be(payload.ProjectDescription);
    }

    [Fact]
    public async Task CreateProject_WithDuplicateName_ReturnsBadRequest()
    {
        // First, create a project
        var createPayload = new
        {
            ProjectName = "Duplicate Name Project",
            ProjectDescription = "First project"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Try to create another project with the same name
        var duplicatePayload = new
        {
            ProjectName = "Duplicate Name Project",
            ProjectDescription = "Second project"
        };

        var response = await _client.PostAsJsonAsync("/api/Projects", duplicatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateProject_WithEmptyName_ReturnsBadRequest()
    {
        var payload = new
        {
            ProjectName = "",
            ProjectDescription = "Description"
        };

        var response = await _client.PostAsJsonAsync("/api/Projects", payload);

        // Should fail validation
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Update Project Tests

    [Fact]
    public async Task UpdateProject_AsOwner_ReturnsNoContent()
    {
        // First, create a project
        var uniqueName = $"Project to Update {Guid.NewGuid()}";
        var createPayload = new
        {
            ProjectName = uniqueName,
            ProjectDescription = "Original description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get all projects to find the one we just created
        var getAllResponse = await _client.GetAsync("/api/Projects?pageNumber=1&pageSize=100");
        var allProjects = await getAllResponse.Content.ReadFromJsonAsync<PagedProjectsResponse>();
        var createdProject = allProjects!.Items.FirstOrDefault(p => p.ProjectName == uniqueName);
        createdProject.Should().NotBeNull();

        // Update the project
        var updatePayload = new
        {
            ProjectName = "Updated Project Name",
            ProjectDescription = "Updated description"
        };

        var response = await _client.PutAsJsonAsync($"/api/Projects/{createdProject!.Id}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update by getting the project
        var getResponse = await _client.GetAsync($"/api/Projects/{createdProject.Id}");
        var updatedProject = await getResponse.Content.ReadFromJsonAsync<ProjectDetailsResponse>();
        updatedProject!.ProjectName.Should().Be("Updated Project Name");
        updatedProject.ProjectDescription.Should().Be("Updated description");
    }

    [Fact]
    public async Task UpdateProject_WithNonExistentId_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var updatePayload = new
        {
            ProjectName = "Updated Name",
            ProjectDescription = "Updated description"
        };

        var response = await _client.PutAsJsonAsync($"/api/Projects/{nonExistentId}", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Delete Project Tests

    [Fact]
    public async Task DeleteProject_AsOwner_ReturnsNoContent()
    {
        // First, create a project
        var uniqueName = $"Project to Delete {Guid.NewGuid()}";
        var createPayload = new
        {
            ProjectName = uniqueName,
            ProjectDescription = "Will be deleted"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Projects", createPayload);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get all projects to find the one we just created
        var getAllResponse = await _client.GetAsync("/api/Projects?pageNumber=1&pageSize=100");
        var allProjects = await getAllResponse.Content.ReadFromJsonAsync<PagedProjectsResponse>();
        var createdProject = allProjects!.Items.FirstOrDefault(p => p.ProjectName == uniqueName);
        createdProject.Should().NotBeNull();

        // Delete the project
        var response = await _client.DeleteAsync($"/api/Projects/{createdProject!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion by trying to get the project
        var getResponse = await _client.GetAsync($"/api/Projects/{createdProject.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProject_WithNonExistentId_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.DeleteAsync($"/api/Projects/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    // Note: Authorization tests for unauthenticated requests would require
    // a different factory setup without TestAuthHandler, which is beyond the scope
    // of these basic integration tests. All endpoints require [Authorize] attribute.

    // Response DTOs for deserialization
    private sealed record PagedProjectsResponse(
        IEnumerable<ProjectItem> Items,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages
    );

    private sealed record ProjectItem(
        Guid Id,
        string ProjectName,
        string ProjectDescription
    );

    private sealed record ProjectDetailsResponse(
        string ProjectName,
        string? ProjectDescription,
        List<object> Users,
        List<object> Tasks
    );

    private sealed record CreateProjectResponse(
        string ProjectName,
        string ProjectDescription
    );
}

