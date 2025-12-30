using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponseDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<TokenResponseDTO?> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for email: {UserEmail}", command.Login.UserEmail);

            var user = await _userRepository.GetByEmailAsync(command.Login.UserEmail);
            // Check if user exists
            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found for email {UserEmail}", command.Login.UserEmail);
                throw new UnauthorizedException("No user found with the provided email address.");
            }

            var hasher = new PasswordHasher<User>();
            // Check if password is correct
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, command.Login.Password);
            if (result != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Login failed: invalid password for email {UserEmail} (UserId: {UserId})", command.Login.UserEmail, user.Id);
                throw new BadRequestException("Invalid email or password.");
            }

            _logger.LogInformation("Login successful for user {UserId} (Email: {UserEmail})", user.Id, command.Login.UserEmail);
            return await _tokenService.CreateTokenResponseAsync(user);
        }
    }
}
