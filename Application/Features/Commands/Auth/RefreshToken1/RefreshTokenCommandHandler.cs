using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Commands.Auth.RefreshToken1
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDTO>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            return await _tokenService.CreateTokenResponseAsync(user);
        }
    }
}
