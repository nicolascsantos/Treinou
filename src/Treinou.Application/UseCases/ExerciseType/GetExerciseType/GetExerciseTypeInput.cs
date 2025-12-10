using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.GetExerciseType
{
    public class GetExerciseTypeInput : IRequest<ExerciseTypeModelOutput>
    {
        public Guid Id { get; set; }

        public GetExerciseTypeInput(Guid id)
            => Id = id;
    }
}
