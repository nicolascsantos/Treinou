using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.GetExercise
{
    public interface IGetExercise : IRequestHandler<GetExerciseInput, ExerciseModelOutput>
    {
    }
}
