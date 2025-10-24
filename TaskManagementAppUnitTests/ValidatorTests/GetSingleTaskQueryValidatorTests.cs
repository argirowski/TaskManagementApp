using Application.Features.Queries.Tasks.GetSingleTask;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class GetSingleTaskQueryValidatorTests
    {
        private readonly GetSingleTaskQueryValidator _validator = new();

        [Fact]
        public void ValidIds_ShouldPassValidation()
        {
            // Arrange
            var query = new GetSingleTaskQuery(Guid.NewGuid(), Guid.NewGuid());
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyProjectId_ShouldFailValidation()
        {
            // Arrange
            var query = new GetSingleTaskQuery(Guid.Empty, Guid.NewGuid());
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "ProjectId");
        }

        [Fact]
        public void EmptyTaskId_ShouldFailValidation()
        {
            // Arrange
            var query = new GetSingleTaskQuery(Guid.NewGuid(), Guid.Empty);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "TaskId");
        }
    }
}
