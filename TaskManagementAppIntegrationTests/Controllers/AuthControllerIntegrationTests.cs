using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagementAppIntegrationTests.Infrastructure;

namespace TaskManagementAppIntegrationTests.Controllers;

public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokens()
    {
        var payload = new
        {
            UserEmail = "test@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<LoginResponse>();
        json.Should().NotBeNull();
        json!.AccessToken.Should().NotBeNullOrWhiteSpace();
        json.RefreshToken.Should().NotBeNullOrWhiteSpace();
        json.UserName.Should().Be("test-user");
    }

    private sealed record LoginResponse(string AccessToken, string RefreshToken, string UserName);
}

