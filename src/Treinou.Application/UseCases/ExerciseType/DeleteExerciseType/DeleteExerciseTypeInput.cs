using MediatR;

namespace Treinou.Application.UseCases.ExerciseType.DeleteExerciseType
{
    public class DeleteExerciseTypeInput : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteExerciseTypeInput(Guid id)
            => Id = id;
    }
}
