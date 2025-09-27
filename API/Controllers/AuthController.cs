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
        public async Task<ActionResult<TokenResponseDTO>> RegisterUser([FromBody] UserDTO userDTO)
        {
            try
            {
                var command = new RegisterUserCommand { User = userDTO };
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(RegisterUser), result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginDTO loginDTO)
        {
            var command = _mapper.Map<LoginUserCommand>(loginDTO);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
