using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TaskManagementAppIntegrationTests.Infrastructure;

/// <summary>
/// Minimal authentication handler for integration tests.
/// Supports multiple users via X-Test-User-Id header.
/// Defaults to the first test user if header is not present.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Default test user IDs
    public const string DefaultUserId = "11111111-1111-1111-1111-111111111111";
    public const string SecondUserId = "22222222-2222-2222-2222-222222222222";
    private const string TestUserIdHeader = "X-Test-User-Id";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Get user ID from header, default to first test user
        var userId = DefaultUserId;
        if (Request.Headers.TryGetValue(TestUserIdHeader, out var userIdHeader))
        {
            userId = userIdHeader.ToString();
        }

        // Map user ID to user details
        var (userName, userEmail) = userId switch
        {
            SecondUserId => ("test-user-2", "test2@example.com"),
            _ => ("test-user", "test@example.com")
        };

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, userEmail)
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

