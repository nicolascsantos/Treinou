using MediatR;

namespace Treinou.Application.UseCases.ExerciseType.DeleteExerciseType
{
    public interface IDeleteExerciseType : IRequestHandler<DeleteExerciseTypeInput, Unit>
    {
    }
}
