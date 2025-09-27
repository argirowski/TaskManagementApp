using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDTO> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(command.User.UserEmail);
            if (existingUser != null)
                throw new InvalidOperationException("User already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = command.User.UserName,
                UserEmail = command.User.UserEmail
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, command.User.Password);

            await _userRepository.AddUserAsync(user);

            // Generate tokens
            var accessToken = _tokenService.CreateToken(user);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = Guid.NewGuid().ToString() // TODO: Implement proper refresh token generation
            };
        }
    }
}
