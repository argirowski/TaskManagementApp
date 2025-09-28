using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Features.Commands.Auth.RefreshToken1;
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
            try
            {
                var command = new RegisterUserCommand { User = userDTO };
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(RegisterUser), result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> LoginUser([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var command = new LoginUserCommand { Login = loginDTO };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            try
            {
                var command = new RefreshTokenCommand { RefreshToken = refreshTokenRequestDTO };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}