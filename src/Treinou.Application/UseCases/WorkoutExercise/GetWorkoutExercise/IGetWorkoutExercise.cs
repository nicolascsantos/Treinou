using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.GetWorkoutExercise
{
    public interface IGetWorkoutExercise : IRequestHandler<GetWorkoutExerciseInput, WorkoutExerciseModelOutput>
    {
    }
}
