using Application.DTOs;
using Application.Features.Commands.Auth.Register;
using Application.Validators;
using FluentAssertions;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                User = new UserDTO
                {
                    UserName = "ValidUser",
                    UserEmail = "user@example.com",
                    Password = "Password1"
                }
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void InvalidUserName_ShouldFail()
        {
            // Arrange
            var invalidNames = new[] { null, "", "AB", new string('A', 21) };
            foreach (var name in invalidNames)
            {
                var command = new RegisterUserCommand
                {
                    User = new UserDTO
                    {
                        UserName = name ?? string.Empty,
                        UserEmail = "user@example.com",
                        Password = "Password1"
                    }
                };
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "User.UserName");
            }
        }

        [Fact]
        public void InvalidEmail_ShouldFail()
        {
            // Arrange
            var invalidEmails = new[] { null, "", "not-an-email", "user@", new string('a', 101) + "@mail.com" };
            foreach (var email in invalidEmails)
            {
                var command = new RegisterUserCommand
                {
                    User = new UserDTO
                    {
                        UserName = "ValidUser",
                        UserEmail = email ?? string.Empty,
                        Password = "Password1"
                    }
                };
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "User.UserEmail");
            }
        }

        [Fact]
        public void Password_TooShort_ShouldFail()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                User = new UserDTO
                {
                    UserName = "ValidUser",
                    UserEmail = "user@example.com",
                    Password = "Sh1"
                }
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "User.Password");
        }

        [Fact]
        public void Password_TooLong_ShouldFail()
        {
            // Arrange
            var longPassword = new string('A', 51) + "1a";
            var command = new RegisterUserCommand
            {
                User = new UserDTO
                {
                    UserName = "ValidUser",
                    UserEmail = "user@example.com",
                    Password = longPassword
                }
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "User.Password");
        }

        [Fact]
        public void Password_MissingNumber_ShouldFail()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                User = new UserDTO
                {
                    UserName = "ValidUser",
                    UserEmail = "user@example.com",
                    Password = "Password"
                }
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "User.Password");
        }
    }
}
