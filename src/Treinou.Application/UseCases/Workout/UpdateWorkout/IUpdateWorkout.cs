using MediatR;
using Treinou.Application.UseCases.Workout.Common;

namespace Treinou.Application.UseCases.Workout.UpdateWorkout
{
    public  interface IUpdateWorkout : IRequestHandler<UpdateWorkoutInput, WorkoutModelOutput>
    {
    }
}
