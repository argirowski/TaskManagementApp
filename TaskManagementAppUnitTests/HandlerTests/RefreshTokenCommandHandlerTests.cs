using Application.DTOs;
using Application.Features.Commands.Auth.RefreshToken;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ILogger<RefreshTokenCommandHandler>> _loggerMock = new();
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerTests()
        {
            _handler = new RefreshTokenCommandHandler(_tokenServiceMock.Object, _userRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsBadRequestException()
        {
            _userRepoMock.Setup(x => x.GetUserByRefreshTokenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((User?)null);

            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "refresh", UserId = Guid.NewGuid() }
            };

            await FluentActions.Invoking(() => _handler.Handle(command, default))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Invalid refresh token.");
        }

        [Fact]
        public async Task Handle_UserFound_ReturnsTokenResponse()
        {
            var user = new User { UserName = "testuser", UserEmail = "test@test.com" };
            _userRepoMock.Setup(x => x.GetUserByRefreshTokenAsync("refresh", user.Id)).ReturnsAsync(user);

            var tokenResponse = new TokenResponseDTO
            {
                AccessToken = "token",
                RefreshToken = "refresh",
                UserName = "testuser"
            };
            _tokenServiceMock.Setup(x => x.CreateTokenResponseAsync(user)).ReturnsAsync(tokenResponse);

            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "refresh", UserId = user.Id }
            };

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.AccessToken.Should().Be("token");
            result.RefreshToken.Should().Be("refresh");
            result.UserName.Should().Be("testuser");
        }
    }
}
