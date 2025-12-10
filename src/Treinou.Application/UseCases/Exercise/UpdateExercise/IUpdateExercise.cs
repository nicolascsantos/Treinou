using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.UpdateExercise
{
    public interface IUpdateExercise : IRequestHandler<UpdateExerciseInput, ExerciseModelOutput>
    {
    }
}
