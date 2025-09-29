using FluentValidation;
using Application.Features.Commands.Auth.Register;

namespace Application.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.User)
                .NotNull().WithMessage("User data is required.");

            RuleFor(x => x.User.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(20).WithMessage("Username must be less than 20 characters.");

            RuleFor(x => x.User.UserEmail)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.User.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 50).WithMessage("Password must be between 6 and 50 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
        }
    }
}