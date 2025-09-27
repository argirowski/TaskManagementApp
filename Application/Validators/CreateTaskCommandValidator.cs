using FluentValidation;
using Application.Features.Commands.Tasks.CreateTask;

namespace Application.Validators
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        //public CreateTaskCommandValidator()
        //{
        //    RuleFor(x => x.ProjectId)
        //        .NotEmpty().WithMessage("Project ID is required.");

        //    RuleFor(x => x.Title)
        //        .NotEmpty().WithMessage("Task title is required.")
        //        .MinimumLength(3).WithMessage("Task title must be at least 3 characters.");

        //    RuleFor(x => x.Description)
        //        .NotEmpty().WithMessage("Task description is required.")
        //        .MinimumLength(3).WithMessage("Task description must be at least 3 characters.");
        //}
    }
}
