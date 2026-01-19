using Treinou.Domain.Entities;

namespace Treinou.Application.DTOs
{
    public class ExerciseTypeDTO
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public ExerciseTypeDTO(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ExerciseTypeDTO FromExerciseType(ExerciseType exerciseType)
            => new(exerciseType.Id, exerciseType.Name);
    }
}
