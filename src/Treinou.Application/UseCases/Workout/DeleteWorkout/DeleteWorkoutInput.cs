using MediatR;

namespace Treinou.Application.UseCases.Workout.DeleteWorkout
{
    public class DeleteWorkoutInput : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteWorkoutInput(Guid id)
            => Id = id;
    }
}
