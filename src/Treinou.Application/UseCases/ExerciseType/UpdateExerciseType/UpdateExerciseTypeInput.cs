using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.UpdateExerciseType
{
    public class UpdateExerciseTypeInput : IRequest<ExerciseTypeModelOutput>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public UpdateExerciseTypeInput(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
