using Application.DTOs;
using Application.Features.Commands.Auth.RefreshToken;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class RefreshTokenCommandValidatorTests
    {
        private readonly RefreshTokenCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_RefreshToken_Is_Null()
        {
            var command = new RefreshTokenCommand { RefreshToken = null! };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
        }

        [Fact]
        public void Should_Have_Error_When_RefreshToken_Value_Is_Empty()
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "", UserId = Guid.NewGuid() }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("RefreshToken.RefreshToken");
        }

        [Fact]
        public void Should_Have_Error_When_RefreshToken_Too_Short()
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "short", UserId = Guid.NewGuid() }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("RefreshToken.RefreshToken");
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "validtoken123", UserId = Guid.Empty }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("RefreshToken.UserId");
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = new RefreshTokenRequestDTO { RefreshToken = "validtoken123", UserId = Guid.NewGuid() }
            };
            var result = _validator.TestValidate(command);
            result.IsValid.Should().BeTrue();
        }
    }
}
