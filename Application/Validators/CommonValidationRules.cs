using FluentValidation;

namespace Application.Validators
{
    public static class CommonValidationRules
    {
        public static IRuleBuilderOptions<T, Guid> ValidProjectId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Project ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID.");
        }

        public static IRuleBuilderOptions<T, Guid> ValidTaskId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Task ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Task ID must be a valid GUID.");
        }

        public static IRuleBuilderOptions<T, Guid> ValidUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");
        }

        public static IRuleBuilderOptions<T, string> ValidProjectName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Project name is required.")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Project name must be no more than 50 characters.");
        }

        public static IRuleBuilderOptions<T, string> ValidTaskTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Task title is required.")
                .MinimumLength(3).WithMessage("Task title must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Task title must be no more than 100 characters.");
        }

        public static IRuleBuilderOptions<T, object> ValidTaskData<T>(this IRuleBuilder<T, object> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage("Task data is required.");
        }

        public static IRuleBuilderOptions<T, string> OptionalDescription<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength = 10, int maxLength = 500)
        {
            return ruleBuilder
                .Length(minLength, maxLength).WithMessage($"Description must be between {minLength} and {maxLength} characters.");
        }
    }
}