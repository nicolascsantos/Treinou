using Treinou.UnitTests.Common;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.WorkoutExercise
{
    [CollectionDefinition(nameof(WorkoutExerciseTestFixture))]
    public class WorkoutExerciseTestFixtureCollection : ICollectionFixture<WorkoutExerciseTestFixture> { }

    public class WorkoutExerciseTestFixture : BaseFixture
    {
        public DomainEntities.WorkoutExercise GetValidWorkoutExercise()
            => new(
                GetValidWorkoutId(),
                GetValidExerciseId(),
                GetValidOrder(),
                GetValidNumberOfSets(),
                GetValidNumberOfRepetitions(),
                GetValidRest(),
                GetValidNotes()
            );

        public Guid GetValidWorkoutId() => Guid.NewGuid();

        public Guid GetValidExerciseId() => Guid.NewGuid();

        public int GetValidOrder() => new Random().Next(1, 10);

        public int GetValidNumberOfSets() => new Random().Next(1, 6);

        public int GetValidNumberOfRepetitions() => new Random().Next(1, 20);

        public TimeSpan GetValidRest() => TimeSpan.FromSeconds(60);

        public string GetValidNotes() => Faker.Lorem.Sentence();
    }
}
