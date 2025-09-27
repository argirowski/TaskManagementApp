using Application.DTOs;
using Application.Features.Commands.Auth.Login;
using Application.Features.Commands.Auth.Register;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] UserDTO userDTO)
        {
            var command = new RegisterUserCommand { User = userDTO };
            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest("User already exists.");
            return CreatedAtAction(nameof(RegisterUser), result);
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
    }
}
