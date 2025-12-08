using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.CreateExercise
{
    public interface ICreateExercise : IRequestHandler<CreateExerciseInput, ExerciseModelOutput>
    {
    }
}
