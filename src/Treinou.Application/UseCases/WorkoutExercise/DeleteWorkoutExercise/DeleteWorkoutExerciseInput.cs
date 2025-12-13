using MediatR;

namespace Treinou.Application.UseCases.WorkoutExercise.DeleteWorkoutExercise
{
    public class DeleteWorkoutExerciseInput : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteWorkoutExerciseInput(Guid id)
            => Id = id;
    }
}
