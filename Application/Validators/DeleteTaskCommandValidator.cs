using FluentValidation;
using Application.Features.Commands.Tasks.DeleteTask;

namespace Application.Validators
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.ProjectId).ValidProjectId();

            RuleFor(x => x.TaskId).ValidTaskId();
        }
    }
}