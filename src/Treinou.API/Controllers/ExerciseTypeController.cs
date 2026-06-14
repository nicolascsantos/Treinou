using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinou.API.Models;
using Treinou.Application.UseCases.ExerciseType.Common;
using Treinou.Application.UseCases.ExerciseType.CreateExerciseType;
using Treinou.Application.UseCases.ExerciseType.DeleteExerciseType;
using Treinou.Application.UseCases.ExerciseType.GetExerciseType;
using Treinou.Application.UseCases.ExerciseType.ListExerciseTypes;
using Treinou.Application.UseCases.ExerciseType.UpdateExerciseType;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExerciseTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExerciseTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<ExerciseTypeModelOutput>))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new GetExerciseTypeInput(id), cancellationToken);
            return Ok(new APIResponse<ExerciseTypeModelOutput>(output));
        }

        [HttpPost]
        [ProducesResponseType(201, StatusCode = StatusCodes.Status201Created, Type = typeof(APIResponse<ExerciseTypeModelOutput>))]
        [ProducesResponseType(422, StatusCode = StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
        [ProducesResponseType(409, StatusCode = StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateExerciseTypeInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(Create), new { output.Id }, output);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204, StatusCode = StatusCodes.Status204NoContent, Type = typeof(void))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new DeleteExerciseTypeInput(id), cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<ListExerciseTypesOutput>))]
        public async Task<IActionResult> Search(
            CancellationToken cancellationToken,
            [FromQuery] int? page = null,
            [FromQuery(Name = "per_page")] int? perPage = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] SearchOrder? dir = null)
        {
            var input = new ListExerciseTypesInput();

            if (page is not null) input.Page = page.Value;
            if (perPage is not null) input.PerPage = perPage.Value;
            if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
            if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
            if (dir is not null) input.Dir = dir.Value;

            var output = await _mediator.Send(input, cancellationToken);

            return Ok(new APIResponseList<ExerciseTypeModelOutput>(output));
        }

        [HttpPut]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<ExerciseTypeModelOutput>))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseTypeInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(new APIResponse<ExerciseTypeModelOutput>(output));
        }
    }
}
