using Application.Features.Commands.Tasks.DeleteTask;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class DeleteTaskCommandValidatorTests
    {
        private readonly DeleteTaskCommandValidator _validator = new();

        [Fact]
        public void ValidIds_ShouldPassValidation()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyProjectId_ShouldFailValidation()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.Empty, Guid.NewGuid());
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "ProjectId");
        }

        [Fact]
        public void EmptyTaskId_ShouldFailValidation()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.Empty);
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "TaskId");
        }
    }
}
