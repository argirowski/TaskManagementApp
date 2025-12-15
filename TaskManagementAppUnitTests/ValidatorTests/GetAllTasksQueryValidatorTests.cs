using Application.Features.Queries.Tasks.GetAllTasks;
using Application.Validators;
using FluentValidation.TestHelper;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class GetAllTasksQueryValidatorTests
    {
        private readonly GetAllTasksQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_ProjectId_Is_Empty()
        {
            var query = new GetAllTasksQuery(Guid.Empty);
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ProjectId_Is_Valid()
        {
            var query = new GetAllTasksQuery(Guid.NewGuid());
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.ProjectId);
        }
    }
}
