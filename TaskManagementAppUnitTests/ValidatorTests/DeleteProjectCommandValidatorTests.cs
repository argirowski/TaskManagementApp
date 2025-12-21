using Application.Features.Commands.Projects.DeleteProject;
using Application.Validators;
using FluentAssertions;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class DeleteProjectCommandValidatorTests
    {
        private readonly DeleteProjectCommandValidator _validator = new();

        [Fact]
        public void ValidId_ShouldPassValidation()
        {
            // Arrange
            var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyId_ShouldFailValidation()
        {
            // Arrange
            var command = new DeleteProjectCommand(Guid.Empty, Guid.NewGuid());
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Id");
        }
    }
}
