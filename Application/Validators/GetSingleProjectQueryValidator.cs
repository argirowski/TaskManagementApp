using FluentValidation;
using Application.Features.Queries.Projects.GetSingleProject;

namespace Application.Validators
{
    public class GetSingleProjectQueryValidator : AbstractValidator<GetSingleProjectQuery>
    {
        public GetSingleProjectQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");
        }
    }
}