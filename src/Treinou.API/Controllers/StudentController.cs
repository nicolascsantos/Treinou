using MediatR;
using Microsoft.AspNetCore.Mvc;
using Treinou.API.Models;
using Treinou.Application.UseCases.Student.Common;
using Treinou.Application.UseCases.Student.GetStudent;

namespace Treinou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id::guid}")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<StudentModelOutput>))]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new GetStudentInput(id), cancellationToken);
            return Ok(new APIResponse<StudentModelOutput>(output));
        }
    }
}
