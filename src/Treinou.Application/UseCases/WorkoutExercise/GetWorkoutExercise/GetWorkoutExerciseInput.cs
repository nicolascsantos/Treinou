using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.GetWorkoutExercise
{
    public class GetWorkoutExerciseInput : IRequest<WorkoutExerciseModelOutput>
    {
        public Guid Id { get; set; }

        public GetWorkoutExerciseInput(Guid id)
            => Id = id;
    }
}
