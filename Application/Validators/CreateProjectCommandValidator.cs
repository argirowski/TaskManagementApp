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
                .MaximumLength(200).WithMessage("Project description must be no more than 200 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
