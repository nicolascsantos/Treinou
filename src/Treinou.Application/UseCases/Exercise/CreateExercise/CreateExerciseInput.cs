using MediatR;
using Treinou.Application.UseCases.Exercise.Common;

namespace Treinou.Application.UseCases.Exercise.CreateExercise
{
    public class CreateExerciseInput : IRequest<ExerciseModelOutput>
    {
        public string Name { get; set; }

        public Guid ExerciseTypeId { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public CreateExerciseInput(
            string name,
            Guid exerciseTypeId,
            bool isActive = true,
            string? imageUrl = null
        )
        {
            Name = name;
            ExerciseTypeId = exerciseTypeId;
            IsActive = isActive;
            ImageUrl = imageUrl;
        }
    }
}
