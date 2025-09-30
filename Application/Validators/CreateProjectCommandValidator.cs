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

            RuleFor(x => x.Project.ProjectName).ValidProjectName();

            RuleFor(x => x.Project.ProjectDescription)
                .Length(10, 200).WithMessage("Project description must be between 10 and 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Project.ProjectDescription));

            RuleFor(x => x.UserId).ValidUserId();
        }
    }
}