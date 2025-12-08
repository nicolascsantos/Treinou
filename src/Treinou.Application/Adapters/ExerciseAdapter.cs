using Treinou.Application.UseCases.Exercise.CreateExercise;
using Treinou.Application.UseCases.Exercise.Common;
using Treinou.Domain.Entities;

namespace Treinou.Application.Adapters
{
    public static class ExerciseAdapter
    {
        public static Exercise ToEntity(CreateExerciseInput input)
        {
            // Create the Exercise entity
            var exercise = new Exercise(
                input.Name,
                input.ExerciseTypeId,
                input.IsActive,
                input.ImageUrl
            );

            return exercise;
        }

        public static ExerciseModelOutput ToOutput(Exercise exercise)
        {
            return ExerciseModelOutput.FromExercise(exercise);
        }
    }
}
