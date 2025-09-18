using MediatR;

namespace Application.Features.Commands.Auth.Register
{
    public class RegisterUserCommand
    : IRequest<bool>
    {
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string Password { get; set; }
    }
}
