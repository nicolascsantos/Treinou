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
        [ProducesResponseType(201, StatusCode = StatusCodes.Status201Created)]
        [ProducesResponseType(422, StatusCode = StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Register([FromBody] RegisterUserInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(Register), new RegisterUserOutput(output.Id, output.Email!, output.UserType));
        }
    }
}
