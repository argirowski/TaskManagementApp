using Application.DTOs;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDTO?> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registration attempt for email: {UserEmail}, username: {UserName}", command.User.UserEmail, command.User.UserName);

            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(command.User.UserEmail);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: user already exists with email {UserEmail}", command.User.UserEmail);
                throw new BadRequestException($"A user with the email '{command.User.UserEmail}' already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = command.User.UserName,
                UserEmail = command.User.UserEmail
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, command.User.Password);

            await _userRepository.AddUserAsync(user);

            _logger.LogInformation("Registration successful for user {UserId} (Email: {UserEmail}, Username: {UserName})", user.Id, user.UserEmail, user.UserName);

            // Map the created user to UserResponseDTO
            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}
