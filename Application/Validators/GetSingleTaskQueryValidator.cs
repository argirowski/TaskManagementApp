using FluentValidation;
using Application.Features.Queries.Tasks.GetSingleTask;

namespace Application.Validators
{
    public class GetSingleTaskQueryValidator : AbstractValidator<GetSingleTaskQuery>
    {
        public GetSingleTaskQueryValidator()
        {
            RuleFor(x => x.ProjectId).ValidProjectId();
            RuleFor(x => x.TaskId).ValidTaskId();
        }
    }
}