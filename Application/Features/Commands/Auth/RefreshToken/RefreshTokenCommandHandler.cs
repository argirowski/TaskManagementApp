using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDTO?>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUserRepository userRepository, ILogger<RefreshTokenCommandHandler> logger)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<TokenResponseDTO?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token request for user {UserId}", request.RefreshToken.UserId);

            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken.RefreshToken, request.RefreshToken.UserId);
            // Check if refresh token is valid
            if (user == null)
            {
                _logger.LogWarning("Refresh token failed: invalid refresh token for user {UserId}", request.RefreshToken.UserId);
                throw new BadRequestException("Invalid refresh token.");
            }

            _logger.LogInformation("Refresh token successful for user {UserId} (Email: {UserEmail})", user.Id, user.UserEmail);
            return await _tokenService.CreateTokenResponseAsync(user);
        }
    }
}
