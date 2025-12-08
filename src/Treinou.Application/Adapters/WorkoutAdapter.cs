using Treinou.Application.UseCases.Workout.CreateWorkout;
using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.Entities;

namespace Treinou.Application.Adapters
{
    
    public static class WorkoutAdapter
    {
        public static Workout ToEntity(CreateWorkoutInput input)
        {
            // Create the Workout entity
            var workout = new Workout(
                input.Name,
                input.TeacherId,
                input.StudentId,
                input.IsActive
            );

            return workout;
        }

        public static WorkoutModelOutput ToOutput(Workout workout)
        {
            return WorkoutModelOutput.FromWorkout(workout);
        }
    }
}
