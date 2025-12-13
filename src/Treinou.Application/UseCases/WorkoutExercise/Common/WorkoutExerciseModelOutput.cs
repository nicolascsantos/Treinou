using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.WorkoutExercise.Common
{
    public class WorkoutExerciseModelOutput
    {
        public Guid Id { get; private set; }

        public Guid ExerciseId { get; private set; }

        public int Order { get; private set; }

        public int NumberOfSets { get; private set; }

        public int NumberOfRepetitions { get; private set; }

        public TimeSpan Rest { get; private set; }

        public string Notes { get; private set; }

        public WorkoutExerciseModelOutput(
            Guid id,
            Guid exerciseId,
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string notes
        )
        {
            Id = id;
            ExerciseId = exerciseId;
            Order = order;
            NumberOfSets = numberOfSets;
            NumberOfRepetitions = numberOfRepetitions;
            Rest = rest;
            Notes = notes;
        }

        public static WorkoutExerciseModelOutput FromWorkoutExercise(Entity.WorkoutExercise workoutExercise)
            => new(
                workoutExercise.Id,
                workoutExercise.ExerciseId,
                workoutExercise.Order,
                workoutExercise.NumberOfSets,
                workoutExercise.NumberOfRepetitions,
                workoutExercise.Rest,
                workoutExercise.Notes
            );
    }
}
