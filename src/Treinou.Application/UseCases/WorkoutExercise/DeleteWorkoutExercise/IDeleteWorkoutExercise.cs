using MediatR;

namespace Treinou.Application.UseCases.WorkoutExercise.DeleteWorkoutExercise
{
    public interface IDeleteWorkoutExercise : IRequestHandler<DeleteWorkoutExerciseInput, Unit>
    {
    }
}
