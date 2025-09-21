using FluentValidation;
using Application.Features.Commands.Tasks.DeleteTask;

namespace Application.Validators
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.");

            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("Task ID is required.");
        }
    }
}
