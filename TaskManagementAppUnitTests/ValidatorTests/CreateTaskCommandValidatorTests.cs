using Application.DTOs;
using Application.Features.Commands.Tasks.CreateTask;
using Application.Validators;
using FluentAssertions;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class CreateTaskCommandValidatorTests
    {
        private readonly CreateTaskCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            // Arrange
            var command = new CreateTaskCommand(
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = "A valid description." }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ProjectId_Empty_ShouldFail()
        {
            // Arrange
            var command = new CreateTaskCommand(
                Guid.Empty,
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = "A valid description." }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "ProjectId");
        }

        [Fact]
        public void TaskTitle_TooShortOrNull_ShouldFail()
        {
            // Arrange
            var invalidTitles = new[] { null, "", "A", "AB" };
            foreach (var title in invalidTitles)
            {
                var command = new CreateTaskCommand(
                    Guid.NewGuid(),
                    new TaskDTO { ProjectTaskTitle = title ?? string.Empty, ProjectTaskDescription = "A valid description." }
                );
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Task.ProjectTaskTitle");
            }
        }

        [Fact]
        public void TaskTitle_TooLong_ShouldFail()
        {
            // Arrange
            var longTitle = new string('A', 101);
            var command = new CreateTaskCommand(
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = longTitle, ProjectTaskDescription = "A valid description." }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Task.ProjectTaskTitle");
        }

        [Fact]
        public void TaskDescription_TooShort_ShouldFail()
        {
            // Arrange
            var invalidDescs = new[] { "short", "123456789" };
            foreach (var desc in invalidDescs)
            {
                var command = new CreateTaskCommand(
                    Guid.NewGuid(),
                    new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = desc }
                );
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Task.ProjectTaskDescription");
            }
        }

        [Fact]
        public void TaskDescription_TooLong_ShouldFail()
        {
            // Arrange
            var longDesc = new string('A', 501);
            var command = new CreateTaskCommand(
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = longDesc }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Task.ProjectTaskDescription");
        }

        [Fact]
        public void TaskDescription_Null_ShouldPass()
        {
            // Arrange
            var command = new CreateTaskCommand(
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = null }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
