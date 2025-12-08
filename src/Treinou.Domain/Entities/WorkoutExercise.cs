using Treinou.Domain.Exceptions;
using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class WorkoutExercise : Entity
    {
        public Guid ExerciseId { get; private set; }

        public Exercise Exercise { get; private set; }

        public int Order { get; private set; }

        public int NumberOfSets { get; private set; }

        public int NumberOfRepetitions { get; private set; }

        public TimeSpan Rest { get; private set; }

        public string Notes { get; private set; }


        public WorkoutExercise(
            Guid exerciseId,
            Exercise exercise,
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string notes
        )
        {
            ExerciseId = exerciseId;
            Exercise = exercise;
            Order = order;
            NumberOfSets = numberOfSets;
            NumberOfRepetitions = numberOfRepetitions;
            Rest = rest;
            Notes = notes;
            Validate();
        }

        protected WorkoutExercise() {}

        public void Validate()
        {
            DomainValidation.NotNull(ExerciseId, nameof(ExerciseId));

            if (Order <= 0)
                throw new EntityValidationException("Order should be greater than zero.");

            if (NumberOfSets <= 0)
                throw new EntityValidationException($"{nameof(NumberOfSets)} should be greater than zero.");

            if (NumberOfRepetitions <= 0)
                throw new EntityValidationException($"{nameof(NumberOfRepetitions)} should be greater than zero.");

            if (Rest.TotalMilliseconds <= 0)
                throw new EntityValidationException($"{nameof(Rest)} should be greater than zero.");
        }

        public void Update(
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string? notes = null
        )
        {
            Order = order;
            NumberOfSets = numberOfSets;
            NumberOfRepetitions = numberOfRepetitions;
            Rest = rest;
            Notes = notes ?? string.Empty;
        }
    }
}
