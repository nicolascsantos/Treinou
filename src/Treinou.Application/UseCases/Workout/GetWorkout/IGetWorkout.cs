using MediatR;
using Treinou.Application.UseCases.Workout.Common;

namespace Treinou.Application.UseCases.Workout.GetWorkout
{
    public interface IGetWorkout : IRequestHandler<GetWorkoutInput, WorkoutModelOutput>
    {
    }
}
