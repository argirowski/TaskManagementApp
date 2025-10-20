using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<TokenResponseDTO?>
    {
        public required RefreshTokenRequestDTO RefreshToken { get; set; }
    }
}
