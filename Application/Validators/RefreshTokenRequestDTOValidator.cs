using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class RefreshTokenRequestDTOValidator : AbstractValidator<RefreshTokenRequestDTO>
    {
        public RefreshTokenRequestDTOValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .Length(32, 512).WithMessage("Refresh token format is invalid.");
        }
    }
}