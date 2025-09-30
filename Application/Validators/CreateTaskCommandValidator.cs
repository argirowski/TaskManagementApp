using FluentValidation;
using Application.Features.Commands.Tasks.CreateTask;

namespace Application.Validators
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId).ValidProjectId();

            RuleFor(x => x.Task)
                .NotNull().WithMessage("Task data is required.");

            RuleFor(x => x.Task.ProjectTaskTitle).ValidTaskTitle();

            RuleFor(x => x.Task.ProjectTaskDescription)
                .Length(10, 500).WithMessage("Task description must be between 10 and 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Task.ProjectTaskDescription));
        }
    }
}