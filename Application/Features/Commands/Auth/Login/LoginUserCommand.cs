using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.Auth.Login
{
    public class LoginUserCommand : IRequest<TokenResponseDTO?>
    {
        public required LoginDTO Login { get; set; }
    }
}
