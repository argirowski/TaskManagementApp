using FluentValidation;
using Application.Features.Commands.Projects.UpdateProject;

namespace Application.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.");

            RuleFor(x => x.Project)
                .NotNull().WithMessage("Project data is required.");

            RuleFor(x => x.Project.ProjectName)
                .NotEmpty().WithMessage("Project name is required.")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters.");
        }
    }
}
