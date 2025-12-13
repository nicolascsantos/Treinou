using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.CreateWorkoutExercise
{
    public interface ICreateWorkoutExercise : IRequestHandler<CreateWorkoutExerciseInput, WorkoutExerciseModelOutput>
    {
    }
}
