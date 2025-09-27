using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommand
    : IRequest<TokenResponseDTO>
    {
        public required UserDTO User { get; set; }
    }
}
