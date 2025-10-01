using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class LoginUserCommandValidatorTests
    {
        private readonly LoginUserCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            var command = new LoginUserCommand
            {
                Login = new LoginDTO
                {
                    UserEmail = "user@example.com",
                    Password = "Password1"
                }
            };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void InvalidEmail_ShouldFail()
        {
            var invalidEmails = new[] { null, "", "not-an-email", "user@", new string('a', 101) + "@mail.com" };
            foreach (var email in invalidEmails)
            {
                var command = new LoginUserCommand
                {
                    Login = new LoginDTO
                    {
                        UserEmail = email ?? string.Empty,
                        Password = "Password1"
                    }
                };
                var result = _validator.Validate(command);
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Login.UserEmail");
            }
        }

        [Fact]
        public void Password_TooShort_ShouldFail()
        {
            var command = new LoginUserCommand
            {
                Login = new LoginDTO
                {
                    UserEmail = "user@example.com",
                    Password = "Sh1"
                }
            };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Login.Password");
        }

        [Fact]
        public void Password_TooLong_ShouldFail()
        {
            var longPassword = new string('A', 51) + "1a";
            var command = new LoginUserCommand
            {
                Login = new LoginDTO
                {
                    UserEmail = "user@example.com",
                    Password = longPassword
                }
            };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Login.Password");
        }

        [Fact]
        public void Password_MissingNumber_ShouldFail()
        {
            var command = new LoginUserCommand
            {
                Login = new LoginDTO
                {
                    UserEmail = "user@example.com",
                    Password = "Password"
                }
            };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Login.Password");
        }
    }
}
