using MediatR;

namespace Treinou.Application.UseCases.WorkoutExercise.ListWorkoutExercises
{
    public interface IListWorkoutExercises : IRequestHandler<ListWorkoutExerciseInput, ListWorkoutExercisesOutput>
    {
    }
}
