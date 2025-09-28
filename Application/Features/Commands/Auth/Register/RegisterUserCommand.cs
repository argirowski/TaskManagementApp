using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommand
    : IRequest<UserResponseDTO>
    {
        public required UserDTO User { get; set; }
    }
}
