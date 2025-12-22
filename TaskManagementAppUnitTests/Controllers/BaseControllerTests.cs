using System.Security.Claims;
using API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementAppUnitTests.Controllers;

public class BaseControllerTests
{
    // Test controller that inherits from BaseController
    private class TestController : BaseController
    {
        public Guid? PublicGetCurrentUserId() => GetCurrentUserId();
    }

    #region GetCurrentUserId Tests

    [Fact]
    public void GetCurrentUserId_WithValidClaim_ReturnsUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var controller = new TestController();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(userId);
    }

    [Fact]
    public void GetCurrentUserId_WithInvalidClaim_ReturnsNull()
    {
        // Arrange
        var controller = new TestController();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "not-a-valid-guid")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUserId_WithNoClaim_ReturnsNull()
    {
        // Arrange
        var controller = new TestController();
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUserId_WithNullClaimValue_ReturnsNull()
    {
        // Arrange
        var controller = new TestController();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUserId_WithEmptyGuid_ReturnsEmptyGuid()
    {
        // Arrange
        var controller = new TestController();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetCurrentUserId_WithMultipleClaims_ReturnsNameIdentifierClaim()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var controller = new TestController();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "test-user"),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        // Act
        var result = controller.PublicGetCurrentUserId();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(userId);
    }

    #endregion
}

