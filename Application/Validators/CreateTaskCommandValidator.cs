using FluentValidation;
using Application.Features.Commands.Tasks.CreateTask;

namespace Application.Validators
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");

            RuleFor(x => x.Task)
                .NotNull().WithMessage("Task data is required.");

            RuleFor(x => x.Task.ProjectTaskTitle)
                .NotEmpty().WithMessage("Task title is required.")
                .MinimumLength(3).WithMessage("Task title must be at least 3 characters.")
                .MaximumLength(30).WithMessage("Task title must be no more than 30 characters.");

            RuleFor(x => x.Task.ProjectTaskDescription)
                .Length(10, 500).WithMessage("Task description must be between 10 and 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Task.ProjectTaskDescription));
        }
    }
}