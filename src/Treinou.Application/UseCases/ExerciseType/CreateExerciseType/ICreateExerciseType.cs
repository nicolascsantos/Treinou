using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.CreateExerciseType
{
    public interface ICreateExerciseType : IRequestHandler<CreateExerciseTypeInput, ExerciseTypeModelOutput>
    {
    }
}
