using Application.DTOs;
using Application.Features.Commands.Tasks.UpdateTask;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class UpdateTaskCommandValidatorTests
    {
        private readonly UpdateTaskCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            // Arrange
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = "A valid description." }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyProjectId_ShouldFailValidation()
        {
            // Arrange
            var command = new UpdateTaskCommand(
                Guid.Empty,
                Guid.NewGuid(),
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = "A valid description." }
            );
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
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
                Guid.Empty,
                new TaskDTO { ProjectTaskTitle = "Valid Title", ProjectTaskDescription = "A valid description." }
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "TaskId");
        }

        [Fact]
        public void TaskTitle_TooShortOrNull_ShouldFailValidation()
        {
            // Arrange
            var invalidTitles = new[] { null, "", "A", "AB" };
            foreach (var title in invalidTitles)
            {
                var command = new UpdateTaskCommand(
                    Guid.NewGuid(),
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
        public void TaskTitle_TooLong_ShouldFailValidation()
        {
            // Arrange
            var longTitle = new string('A', 101);
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
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
        public void TaskDescription_TooShort_ShouldFailValidation()
        {
            // Arrange
            var invalidDescs = new[] { "short", "123456789" };
            foreach (var desc in invalidDescs)
            {
                var command = new UpdateTaskCommand(
                    Guid.NewGuid(),
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
        public void TaskDescription_TooLong_ShouldFailValidation()
        {
            // Arrange
            var longDesc = new string('A', 501);
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
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
        public void TaskDescription_Null_ShouldPassValidation()
        {
            // Arrange
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
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
