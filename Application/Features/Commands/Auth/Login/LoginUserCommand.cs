namespace Application.Features.Commands.Auth.Login
{
    using MediatR;

    public class LoginUserCommand : IRequest<string>
    {
        public required string UserEmail { get; set; }
        public required string Password { get; set; }
    }
}
