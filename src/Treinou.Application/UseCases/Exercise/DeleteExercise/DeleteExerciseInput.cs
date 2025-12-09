using MediatR;

namespace Treinou.Application.UseCases.Exercise.DeleteExercise
{
    public class DeleteExerciseInput : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteExerciseInput(Guid id)
            => Id = id;
    }
}
