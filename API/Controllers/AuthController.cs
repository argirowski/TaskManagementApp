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
        public async Task<ActionResult<string>> Register([FromBody] UserDTO userDTO)
        {
            var command = _mapper.Map<RegisterUserCommand>(userDTO);
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("User already exists.");
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO loginDTO)
        {
            var command = _mapper.Map<LoginUserCommand>(loginDTO);
            var result = await _mediator.Send(command);
            if (string.IsNullOrEmpty(result))
                return Unauthorized("Invalid email or password.");
            return Ok(result);
        }
    }
}
