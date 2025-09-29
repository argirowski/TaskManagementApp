using FluentValidation;
using Application.Features.Commands.Projects.UpdateProject;

namespace Application.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");

            RuleFor(x => x.Project)
                .NotNull().WithMessage("Project data is required.");

            RuleFor(x => x.Project.ProjectName)
                .NotEmpty().WithMessage("Project name is required.")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters.")
                .MaximumLength(30).WithMessage("Project name must be no more than 30 characters.");

            RuleFor(x => x.Project.ProjectDescription)
                .NotEmpty().WithMessage("Project description is required.")
                .Length(10, 200).WithMessage("Project description must be between 10 and 200 characters.");
        }
    }
}