using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<ILogger<LoginUserCommandHandler>> _loggerMock = new();
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _handler = new LoginUserCommandHandler(_userRepoMock.Object, _tokenServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedException()
        {
            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var command = new LoginUserCommand { Login = new LoginDTO { UserEmail = "test@test.com", Password = "pass" } };

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.UnauthorizedException>()
                .WithMessage("No user found with the provided email address.");
        }

        [Fact]
        public async Task Handle_WrongPassword_ThrowsBadRequestException()
        {
            var user = new User { UserName = "testuser", UserEmail = "test@test.com", PasswordHash = new PasswordHasher<User>().HashPassword(null, "correct") };
            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var command = new LoginUserCommand { Login = new LoginDTO { UserEmail = "test@test.com", Password = "wrong" } };

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Application.Exceptions.BadRequestException>()
                .WithMessage("Invalid email or password.");
        }

        [Fact]
        public async Task Handle_CorrectPassword_ReturnsTokenResponse()
        {
            var user = new User
            {
                UserName = "testuser",
                UserEmail = "test@test.com",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "correct")
            };
            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var tokenResponse = new TokenResponseDTO
            {
                AccessToken = "token",
                RefreshToken = "refresh",
                UserName = "testuser"
            };
            _tokenServiceMock.Setup(x => x.CreateTokenResponseAsync(user)).ReturnsAsync(tokenResponse);
            var command = new LoginUserCommand { Login = new LoginDTO { UserEmail = "test@test.com", Password = "correct" } };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.AccessToken.Should().Be("token");
            result.RefreshToken.Should().Be("refresh");
            result.UserName.Should().Be("testuser");
        }
    }
}
