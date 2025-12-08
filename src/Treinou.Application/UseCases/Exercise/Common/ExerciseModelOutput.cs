namespace Treinou.Application.UseCases.Exercise.Common
{
    public class ExerciseModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public Guid ExerciseTypeId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public ExerciseModelOutput(
            Guid id,
            string name,
            string? imageUrl,
            Guid exerciseTypeId,
            bool isActive,
            DateTime createdAt
        )
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            ExerciseTypeId = exerciseTypeId;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static ExerciseModelOutput FromExercise(Domain.Entities.Exercise exercise)
        {
            return new ExerciseModelOutput(
                exercise.Id,
                exercise.Name,
                exercise.ImageUrl,
                exercise.ExcerciseTypeId,
                exercise.IsActive,
                exercise.CreatedAt
            );
        }
    }
}
