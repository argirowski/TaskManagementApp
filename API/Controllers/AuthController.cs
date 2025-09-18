using Application.DTOs;
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
        private readonly AutoMapper.IMapper _mapper;

        public AuthController(IMediator mediator, AutoMapper.IMapper mapper)
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
    }
}
