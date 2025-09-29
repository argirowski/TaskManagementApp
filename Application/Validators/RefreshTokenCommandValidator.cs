using FluentValidation;
using Application.Features.Commands.Auth.RefreshToken1;

namespace Application.Validators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotNull().WithMessage("Refresh token data is required.");

            RuleFor(x => x.RefreshToken.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(x => x.RefreshToken.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(10).WithMessage("Refresh token must be at least 10 characters.")
                .MaximumLength(500).WithMessage("Refresh token must be less than 500 characters.");
        }
    }
}