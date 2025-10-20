using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Features.Commands.Auth.RefreshToken;
using Application.Features.Commands.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> RegisterUser([FromBody] UserDTO userDTO)
        {
            var command = new RegisterUserCommand { User = userDTO };
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest("User with this email already exists.");
            }

            return CreatedAtAction(nameof(RegisterUser), result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> LoginUser([FromBody] LoginDTO loginDTO)
        {
            var command = new LoginUserCommand { Login = loginDTO };
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            var command = new RefreshTokenCommand { RefreshToken = refreshTokenRequestDTO };
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }
    }
}