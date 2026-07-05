using MediatR;
using Microsoft.AspNetCore.Mvc;
using Treinou.Application.UseCases.Auth;

namespace Treinou.API.Controllers
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
        [ProducesResponseType(typeof(RegisterUserOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] RegisterUserInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(Register), output);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);
        }
    }
}
