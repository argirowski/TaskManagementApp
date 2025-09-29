using FluentValidation;
using Application.Features.Commands.Auth.Login;

namespace Application.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Login)
                .NotNull().WithMessage("Login data is required.");

            RuleFor(x => x.Login.UserEmail)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Login.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 50).WithMessage("Password must be between 6 and 50 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
        }
    }
}