using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDTO?>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<TokenResponseDTO?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken.RefreshToken, request.RefreshToken.UserId);
            // Check if refresh token is valid
            if (user == null)
            {
                throw new BadRequestException("Invalid refresh token.");
            }

            return await _tokenService.CreateTokenResponseAsync(user);
        }
    }
}
