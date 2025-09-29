using FluentValidation;
using Application.Features.Queries.Projects.GetAllProjects;

namespace Application.Validators
{
    public class GetAllProjectsQueryValidator : AbstractValidator<GetAllProjectsQuery>
    {
        public GetAllProjectsQueryValidator()
        {
            // No validation rules needed for this parameterless query
            // This validator exists for consistency with the validation pipeline
        }
    }
}