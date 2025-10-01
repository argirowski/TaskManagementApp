using Application.DTOs;
using Application.Features.Commands.Projects.CreateProject;
using Application.Validators;
using FluentAssertions;
using System;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class CreateProjectCommandValidatorTests
    {
        private readonly CreateProjectCommandValidator _validator = new();

        [Fact]
        public void ValidCommand_ShouldPassValidation()
        {
            var command = new CreateProjectCommand(
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = "A valid project description." },
                Guid.NewGuid()
            );
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ProjectName_TooShortOrNull_ShouldFail()
        {
            var invalidNames = new[] { null, string.Empty, "A", "AB" };
            foreach (var name in invalidNames)
            {
                var command = new CreateProjectCommand(
                    new CreateProjectDTO { ProjectName = name ?? string.Empty, ProjectDescription = "A valid project description." },
                    Guid.NewGuid()
                );
                var result = _validator.Validate(command);
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectName");
            }
        }

        [Fact]
        public void ProjectName_TooLong_ShouldFail()
        {
            var longName = new string('A', 51);
            var command = new CreateProjectCommand(
                new CreateProjectDTO { ProjectName = longName, ProjectDescription = "A valid project description." },
                Guid.NewGuid()
            );
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectName");
        }

        [Fact]
        public void ProjectDescription_TooShort_ShouldFail()
        {
            var invalidDescs = new[] { "short", "123456789" }; // 5 and 9 chars
            foreach (var desc in invalidDescs)
            {
                var command = new CreateProjectCommand(
                    new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = desc },
                    Guid.NewGuid()
                );
                var result = _validator.Validate(command);
                result.IsValid.Should().BeFalse();
                result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectDescription");
            }
        }

        [Fact]
        public void ProjectDescription_TooLong_ShouldFail()
        {
            var longDesc = new string('A', 201);
            var command = new CreateProjectCommand(
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = longDesc },
                Guid.NewGuid()
            );
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Project.ProjectDescription");
        }

        [Fact]
        public void ProjectDescription_Null_ShouldPass()
        {
            var command = new CreateProjectCommand(
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = null },
                Guid.NewGuid()
            );
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void UserId_Empty_ShouldFail()
        {
            var command = new CreateProjectCommand(
                new CreateProjectDTO { ProjectName = "Valid Name", ProjectDescription = "A valid project description." },
                Guid.Empty
            );
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "UserId");
        }
    }
}
