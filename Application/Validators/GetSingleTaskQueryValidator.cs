using FluentValidation;
using Application.Features.Queries.Tasks.GetSingleTask;

namespace Application.Validators
{
    public class GetSingleTaskQueryValidator : AbstractValidator<GetSingleTaskQuery>
    {
        public GetSingleTaskQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");

            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("Task ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Task ID must be a valid GUID.");
        }
    }
}