using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.UpdateExercise
{
    public class UpdateExerciseInput : IRequest<ExerciseModelOutput>
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public Guid ExerciseTypeId { get; set; }
    }
}
