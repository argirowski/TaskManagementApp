using FluentValidation;
using Application.Features.Commands.Projects.CreateProject;

namespace Application.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Project)
                .NotNull().WithMessage("Project data is required.");

            RuleFor(x => x.Project.ProjectName)
                .NotEmpty().WithMessage("Project name is required.")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters.")
                .MaximumLength(30).WithMessage("Project name must be no more than 30 characters.");

            RuleFor(x => x.Project.ProjectDescription)
                .Length(10, 200).WithMessage("Project description must be between 10 and 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Project.ProjectDescription));

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");
        }
    }
}