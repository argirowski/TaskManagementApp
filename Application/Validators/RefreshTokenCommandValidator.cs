using FluentValidation;
using Application.Features.Commands.Auth.RefreshToken;

namespace Application.Validators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotNull().WithMessage("Refresh token data is required.");

            RuleFor(x => x.RefreshToken.UserId).ValidUserId();

            RuleFor(x => x.RefreshToken.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(10).WithMessage("Refresh token must be at least 10 characters.")
                .MaximumLength(500).WithMessage("Refresh token must be less than 500 characters.");
        }
    }
}