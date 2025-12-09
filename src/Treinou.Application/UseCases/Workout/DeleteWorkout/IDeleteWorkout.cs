using MediatR;

namespace Treinou.Application.UseCases.Workout.DeleteWorkout
{
    public interface IDeleteWorkout : IRequestHandler<DeleteWorkoutInput, Unit>
    {
    }
}
