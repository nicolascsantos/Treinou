using MediatR;
using Treinou.Application.UseCases.Workout.Common;

namespace Treinou.Application.UseCases.Workout.GetWorkout
{
    public class GetWorkoutInput : IRequest<WorkoutModelOutput>
    {
        public Guid Id { get; private set; }

        public GetWorkoutInput(Guid id)
            => Id = id;
    }
}
