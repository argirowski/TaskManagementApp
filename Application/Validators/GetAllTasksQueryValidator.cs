using FluentValidation;
using Application.Features.Queries.Tasks.GetAllTasks;

namespace Application.Validators
{
    public class GetAllTasksQueryValidator : AbstractValidator<GetAllTasksQuery>
    {
        public GetAllTasksQueryValidator()
        {
            RuleFor(x => x.ProjectId).ValidProjectId();
        }
    }
}