using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.UpdateExerciseType
{
    public interface IUpdateExerciseType : IRequestHandler<UpdateExerciseTypeInput, ExerciseTypeModelOutput>
    {
    }
}
