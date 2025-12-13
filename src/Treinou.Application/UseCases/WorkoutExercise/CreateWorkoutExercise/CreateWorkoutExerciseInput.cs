using MediatR;
using Treinou.Application.UseCases.WorkoutExercise.Common;

namespace Treinou.Application.UseCases.WorkoutExercise.CreateWorkoutExercise
{
    public class CreateWorkoutExerciseInput : IRequest<WorkoutExerciseModelOutput>
    {
        public Guid ExerciseId { get; private set; }

        public int Order { get; private set; }

        public int NumberOfSets { get; private set; }

        public int NumberOfRepetitions { get; private set; }

        public TimeSpan Rest { get; private set; }

        public string Notes { get; private set; }

        public CreateWorkoutExerciseInput(
            Guid exerciseId,
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string notes
        )
        {
            ExerciseId = exerciseId;
            Order = order;
            NumberOfSets = numberOfSets;
            NumberOfRepetitions = numberOfRepetitions;
            Rest = rest;
            Notes = notes;
        }
    }
}
