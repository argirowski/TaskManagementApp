using FluentValidation;
using Application.Features.Commands.Projects.DeleteProject;

namespace Application.Validators
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");
        }
    }
}