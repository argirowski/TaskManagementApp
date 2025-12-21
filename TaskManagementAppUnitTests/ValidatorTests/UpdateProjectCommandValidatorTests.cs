using Application.DTOs;
using Application.Features.Commands.Projects.UpdateProject;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class UpdateProjectCommandValidatorTests
    {
        private readonly UpdateProjectCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            // Arrange
            var command = new UpdateProjectCommand(
                Guid.NewGuid(),
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = "A valid project description." },
                Guid.NewGuid()
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyId_ShouldFailValidation()
        {
            // Arrange
            var command = new UpdateProjectCommand(
                Guid.Empty,
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = "A valid project description." },
                Guid.NewGuid()
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Id");
        }

        [Fact]
        public void ProjectName_TooShortOrNull_ShouldFailValidation()
        {
            // Arrange
            var invalidNames = new[] { null, string.Empty, "A", "AB" };
            foreach (var name in invalidNames)
            {
                var command = new UpdateProjectCommand(
                    Guid.NewGuid(),
                    new CreateProjectDTO { ProjectName = name ?? string.Empty, ProjectDescription = "A valid project description." },
                    Guid.NewGuid()
                );
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectName");
            }
        }

        [Fact]
        public void ProjectName_TooLong_ShouldFailValidation()
        {
            // Arrange
            var longName = new string('A', 51);
            var command = new UpdateProjectCommand(
                Guid.NewGuid(),
                new CreateProjectDTO { ProjectName = longName, ProjectDescription = "A valid project description." },
                Guid.NewGuid()
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectName");
        }

        [Fact]
        public void ProjectDescription_TooShort_ShouldFailValidation()
        {
            // Arrange
            var invalidDescs = new[] { "short", "123456789" };
            foreach (var desc in invalidDescs)
            {
                var command = new UpdateProjectCommand(
                    Guid.NewGuid(),
                    new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = desc },
                    Guid.NewGuid()
                );
                // Act
                var result = _validator.Validate(command);
                // Assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectDescription");
            }
        }

        [Fact]
        public void ProjectDescription_TooLong_ShouldFailValidation()
        {
            // Arrange
            var longDesc = new string('A', 201);
            var command = new UpdateProjectCommand(
                Guid.NewGuid(),
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = longDesc },
                Guid.NewGuid()
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectDescription");
        }

        [Fact]
        public void ProjectDescription_Null_ShouldPassValidation()
        {
            // Arrange
            var command = new UpdateProjectCommand(
                Guid.NewGuid(),
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = null },
                Guid.NewGuid()
            );
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
