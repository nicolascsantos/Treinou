using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.UpdateWorkoutExercise
{
    public interface IUpdateWorkoutExercise : IRequestHandler<UpdateWorkoutExerciseInput, WorkoutExerciseModelOutput>
    {
    }
}
