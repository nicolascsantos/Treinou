using MediatR;
using Treinou.Application.UseCases.Workout.Common;

namespace Treinou.Application.UseCases.Workout.CreateWorkout
{
    public interface ICreateWorkout : IRequestHandler<CreateWorkoutInput, WorkoutModelOutput>
    {
    }
}
