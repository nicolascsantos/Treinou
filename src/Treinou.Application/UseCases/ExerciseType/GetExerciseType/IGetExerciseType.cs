using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.GetExerciseType
{
    public interface IGetExerciseType : IRequestHandler<GetExerciseTypeInput, ExerciseTypeModelOutput>
    {
    }
}
