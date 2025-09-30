using FluentValidation;
using Application.Features.Commands.Projects.DeleteProject;

namespace Application.Validators
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(x => x.Id).ValidProjectId();
        }
    }
}