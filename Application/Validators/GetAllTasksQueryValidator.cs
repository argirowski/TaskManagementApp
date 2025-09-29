using FluentValidation;
using Application.Features.Queries.Tasks.GetAllTasks;

namespace Application.Validators
{
    public class GetAllTasksQueryValidator : AbstractValidator<GetAllTasksQuery>
    {
        public GetAllTasksQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");
        }
    }
}