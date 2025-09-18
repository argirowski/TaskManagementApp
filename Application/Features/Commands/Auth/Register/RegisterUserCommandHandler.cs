using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(command.UserEmail);
            if (existingUser != null)
                return false;

            // Hash password (simple example, use a secure method in production)
            var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(command.Password));

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = command.UserName,
                UserEmail = command.UserEmail,
                PasswordHash = passwordHash
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }
    }
}
