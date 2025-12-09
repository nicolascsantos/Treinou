using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.DeleteExercise
{
    public interface IDeleteExercise : IRequestHandler<DeleteExerciseInput, Unit>
    {
    }
}
