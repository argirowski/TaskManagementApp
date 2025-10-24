using Application.Features.Queries.Projects.GetSingleProject;
using Application.Validators;
using FluentAssertions;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class GetSingleProjectQueryValidatorTests
    {
        private readonly GetSingleProjectQueryValidator _validator = new();

        [Fact]
        public void ValidId_ShouldPassValidation()
        {
            // Arrange
            var query = new GetSingleProjectQuery(Guid.NewGuid());
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void EmptyId_ShouldFailValidation()
        {
            // Arrange
            var query = new GetSingleProjectQuery(Guid.Empty);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Id");
        }
    }
}
