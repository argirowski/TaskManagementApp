using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class TaskDTOValidator : AbstractValidator<TaskDTO>
    {
        public TaskDTOValidator()
        {
            RuleFor(x => x.ProjectTaskTitle)
                .NotEmpty().WithMessage("Task title is required.")
                .Length(3, 30).WithMessage("Task title must be between 3 and 30 characters.");

            RuleFor(x => x.ProjectTaskDescription)
                .NotEmpty().WithMessage("Task description is required.")
                .Length(10, 500).WithMessage("Task description must be between 10 and 500 characters.");
        }
    }
}