using Application.Features.Queries.Projects.GetAllProjects;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class GetAllProjectsQueryValidatorTests
    {
        private readonly GetAllProjectsQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Any_Query()
        {
            var query = new GetAllProjectsQuery();
            var result = _validator.TestValidate(query);
            result.IsValid.Should().BeTrue();
        }
    }
}
