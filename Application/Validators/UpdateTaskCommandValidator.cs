using FluentValidation;
using Application.Features.Commands.Tasks.UpdateTask;

namespace Application.Validators
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId).ValidProjectId();

            RuleFor(x => x.TaskId).ValidTaskId();

            RuleFor(x => x.Task).ValidTaskData();

            RuleFor(x => x.Task.ProjectTaskTitle).ValidTaskTitle();

            RuleFor(x => x.Task.ProjectTaskDescription)
                .Length(10, 500).WithMessage("Task description must be between 10 and 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Task.ProjectTaskDescription));
        }
    }
}