using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinou.Application.UseCases.WeightEntry.CreateWeightEntry;

namespace Treinou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeightEntryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeightEntryController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody] CreateWeightEntryInput request, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(request, cancellationToken);
            return CreatedAtAction(nameof(Create), new { output.Id }, output);
        }
    }
}
