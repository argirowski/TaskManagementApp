using FluentValidation;
using Application.Features.Queries.Projects.GetSingleProject;

namespace Application.Validators
{
    public class GetSingleProjectQueryValidator : AbstractValidator<GetSingleProjectQuery>
    {
        public GetSingleProjectQueryValidator()
        {
            RuleFor(x => x.Id).ValidProjectId();
        }
    }
}