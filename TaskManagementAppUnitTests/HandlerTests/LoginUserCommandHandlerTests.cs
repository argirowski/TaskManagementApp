using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _handler = new LoginUserCommandHandler(_userRepoMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsNull()
        {
            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var command = new LoginUserCommand { Login = new LoginDTO { UserEmail = "test@test.com", Password = "pass" } };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WrongPassword_ReturnsNull()
        {
            var user = new User { UserName = "testuser", UserEmail = "test@test.com", PasswordHash = new PasswordHasher<User>().HashPassword(null, "correct") };
            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var command = new LoginUserCommand { Login = new LoginDTO { UserEmail = "test@test.com", Password = "wrong" } };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeNull();
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
