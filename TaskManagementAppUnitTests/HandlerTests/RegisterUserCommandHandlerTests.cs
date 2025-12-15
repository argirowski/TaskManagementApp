using Application.DTOs;
using Application.Features.Commands.Auth.Register;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TaskManagementAppUnitTests.HandlerTests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _handler = new RegisterUserCommandHandler(_userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UserAlreadyExists_ReturnsNull()
        {
            // Arrange
            var existingUser = new User { UserName = "existing", UserEmail = "test@test.com" };
            _userRepoMock.Setup(x => x.GetByEmailAsync("test@test.com")).ReturnsAsync(existingUser);
            var command = new RegisterUserCommand
            {
                User = new UserDTO { UserName = "new", UserEmail = "test@test.com", Password = "pass" }
            };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_NewUser_AddsUserAndReturnsDto()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByEmailAsync("test@test.com")).ReturnsAsync((User?)null);
            _userRepoMock.Setup(x => x.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var command = new RegisterUserCommand
            {
                User = new UserDTO { UserName = "new", UserEmail = "test@test.com", Password = "pass" }
            };

            var user = new User { UserName = "new", UserEmail = "test@test.com" };
            var userResponse = new UserResponseDTO { UserName = "new", UserEmail = "test@test.com" };

            _mapperMock.Setup(x => x.Map<UserResponseDTO>(It.IsAny<User>())).Returns(userResponse);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("new");
            result.UserEmail.Should().Be("test@test.com");
            _userRepoMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
