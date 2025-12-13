using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.UpdateWorkoutExercise
{
    public class UpdateWorkoutExerciseInput : IRequest<WorkoutExerciseModelOutput>
    {
        public Guid Id { get; private set; }

        public Guid ExerciseId { get; private set; }

        public int Order { get; private set; }

        public int NumberOfSets { get; private set; }

        public int NumberOfRepetitions { get; private set; }

        public TimeSpan Rest { get; private set; }

        public string Notes { get; private set; }

        public UpdateWorkoutExerciseInput(
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
    }
}
