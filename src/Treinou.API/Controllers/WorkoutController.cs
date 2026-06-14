using MediatR;
using Microsoft.AspNetCore.Mvc;
using Treinou.API.Models;
using Treinou.Application.UseCases.Workout.Common;
using Treinou.Application.UseCases.Workout.CreateWorkout;
using Treinou.Application.UseCases.Workout.DeleteWorkout;
using Treinou.Application.UseCases.Workout.GetWorkout;
using Treinou.Application.UseCases.Workout.ListWorkouts;
using Treinou.Application.UseCases.Workout.UpdateWorkout;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private IMediator _mediator;

        public WorkoutController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("{id::guid}")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<WorkoutModelOutput>))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new GetWorkoutInput(id), cancellationToken);
            return Ok(new APIResponse<WorkoutModelOutput>(output));
        }


        [HttpPost]
        [ProducesResponseType(201, StatusCode = StatusCodes.Status201Created, Type = typeof(APIResponse<WorkoutModelOutput>))]
        [ProducesResponseType(422, StatusCode = StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
        [ProducesResponseType(409, StatusCode = StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(Create), new { output.Id }, output);
        }

        [HttpDelete("{id::guid}")]
        [ProducesResponseType(204, StatusCode = StatusCodes.Status204NoContent, Type = typeof(void))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new DeleteWorkoutInput(id), cancellationToken);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<WorkoutModelOutput>))]
        [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromBody] UpdateWorkoutInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(new APIResponse<WorkoutModelOutput>(output));
        }

        [HttpGet]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<WorkoutModelOutput>))]
        public async Task<IActionResult> Search(
            CancellationToken cancellationToken,
            [FromQuery] int? page = null,
            [FromQuery(Name = "per_page")] int? perPage = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] SearchOrder? dir = null)
        {
            var input = new ListWorkoutsInput();

            if (page is not null) input.Page = page.Value;
            if (perPage is not null) input.PerPage = perPage.Value;
            if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
            if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
            if (dir is not null) input.Dir = dir.Value;

            var output = await _mediator.Send(input, cancellationToken);

            return Ok(new APIResponseList<WorkoutModelOutput>(output));
        }
    }
}
