using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
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

            // Map the created user to UserResponseDTO
            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}
