using Domain.Interfaces;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;

namespace Application.Features.Commands.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDTO> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(command.Login.UserEmail);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, command.Login.Password);
            if (result != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Invalid email or password.");

            return await _tokenService.CreateTokenResponseAsync(user);
        }
    }
}
