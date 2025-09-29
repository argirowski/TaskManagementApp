using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class CreateProjectDTOValidator : AbstractValidator<CreateProjectDTO>
    {
        public CreateProjectDTOValidator()
        {
            RuleFor(x => x.ProjectName)
                .NotEmpty().WithMessage("Project name is required.")
                .Length(3, 30).WithMessage("Project name must be between 3 and 30 characters.");

            RuleFor(x => x.ProjectDescription)
                .NotEmpty().WithMessage("Project description is required.")
                .Length(10, 200).WithMessage("Project description must be between 10 and 200 characters.");
        }
    }
}