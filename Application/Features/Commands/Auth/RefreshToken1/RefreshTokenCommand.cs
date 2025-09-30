using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Auth.RefreshToken1
{
    public class RefreshTokenCommand : IRequest<TokenResponseDTO?>
    {
        public required RefreshTokenRequestDTO RefreshToken { get; set; }
    }
}
