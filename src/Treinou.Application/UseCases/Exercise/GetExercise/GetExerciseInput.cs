using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.GetExercise
{
    public class GetExerciseInput : IRequest<ExerciseModelOutput>
    {
        public Guid Id { get; private set; }

        public GetExerciseInput(Guid id)
            => Id = id;
    }
}
