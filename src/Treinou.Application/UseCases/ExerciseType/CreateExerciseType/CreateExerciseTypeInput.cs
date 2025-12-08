using MediatR;
using Treinou.Application.UseCases.ExerciseType.Common;

namespace Treinou.Application.UseCases.ExerciseType.CreateExerciseType
{
    public class CreateExerciseTypeInput : IRequest<ExerciseTypeModelOutput>
    {
        public string Name { get; set; }

        public CreateExerciseTypeInput(string name)
        {
            Name = name;
        }
    }
}
