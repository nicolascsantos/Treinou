using Treinou.Application.DTOs;

namespace Treinou.Application.UseCases.Exercise.Common
{
    public class ExerciseModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public ExerciseTypeDTO? ExerciseType { get; set; } = null;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public ExerciseModelOutput(
            Guid id,
            string name,
            string? imageUrl,
            ExerciseTypeDTO exerciseType,
            bool isActive,
            DateTime createdAt
        )
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            ExerciseType = exerciseType;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static ExerciseModelOutput FromExercise(Domain.Entities.Exercise exercise)
        {
            return new ExerciseModelOutput(
                exercise.Id,
                exercise.Name,
                exercise.ImageUrl,
                ExerciseTypeDTO.FromExerciseType(exercise.ExcerciseType) ?? null,
                exercise.IsActive,
                exercise.CreatedAt
            );
        }
    }
}
