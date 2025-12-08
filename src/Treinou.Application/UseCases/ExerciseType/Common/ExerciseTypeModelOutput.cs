namespace Treinou.Application.UseCases.ExerciseType.Common
{
    public class ExerciseTypeModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ExerciseTypeModelOutput(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ExerciseTypeModelOutput FromExerciseType(Domain.Entities.ExerciseType exerciseType)
        {
            return new ExerciseTypeModelOutput(
                exerciseType.Id,
                exerciseType.Name
            );
        }
    }
}
