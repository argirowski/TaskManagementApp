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

    #region Login Tests

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

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsUnauthorized()
    {
        var payload = new
        {
            UserEmail = "nonexistent@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsBadRequest()
    {
        var payload = new
        {
            UserEmail = "test@example.com",
            Password = "WrongPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Register Tests

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        var payload = new
        {
            UserName = "newuser",
            UserEmail = "newuser@example.com",
            Password = "NewPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/register", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadFromJsonAsync<UserResponse>();
        json.Should().NotBeNull();
        json!.UserName.Should().Be("newuser");
        json.UserEmail.Should().Be("newuser@example.com");
        json.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        var payload = new
        {
            UserName = "duplicateuser",
            UserEmail = "test@example.com", // Already exists
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/register", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Refresh Token Tests

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewTokens()
    {
        // First, login to get a valid refresh token
        var loginPayload = new
        {
            UserEmail = "test@example.com",
            Password = "Password123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginPayload);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        loginResult.Should().NotBeNull();

        // Now refresh the token
        var refreshPayload = new
        {
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            RefreshToken = loginResult!.RefreshToken
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/refresh-token", refreshPayload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<LoginResponse>();
        json.Should().NotBeNull();
        json!.AccessToken.Should().NotBeNullOrWhiteSpace();
        json.RefreshToken.Should().NotBeNullOrWhiteSpace();
        json.UserName.Should().Be("test-user");
        
        // Refresh token should always be different (randomly generated)
        json.RefreshToken.Should().NotBe(loginResult.RefreshToken);
        
        // Access token might be the same if generated at the exact same time with same claims
        // but it should be a valid token
        json.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task RefreshToken_WithInvalidToken_ReturnsBadRequest()
    {
        var refreshPayload = new
        {
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            RefreshToken = "invalid-refresh-token"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/refresh-token", refreshPayload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RefreshToken_WithInvalidUserId_ReturnsBadRequest()
    {
        // First, login to get a valid refresh token
        var loginPayload = new
        {
            UserEmail = "test@example.com",
            Password = "Password123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginPayload);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        loginResult.Should().NotBeNull();

        // Try to refresh with wrong user ID
        var refreshPayload = new
        {
            UserId = Guid.NewGuid(), // Wrong user ID
            RefreshToken = loginResult!.RefreshToken
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/refresh-token", refreshPayload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    private sealed record LoginResponse(string AccessToken, string RefreshToken, string UserName);
    private sealed record UserResponse(Guid Id, string UserName, string UserEmail);
}

