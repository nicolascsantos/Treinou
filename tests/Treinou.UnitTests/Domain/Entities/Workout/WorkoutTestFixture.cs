using Treinou.UnitTests.Common;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.Workout
{
    [CollectionDefinition(nameof(WorkoutTestFixture))]
    public class WorkoutTestFixtureCollection : ICollectionFixture<WorkoutTestFixture> { }

    public class WorkoutTestFixture : BaseFixture
    {
        public DomainEntities.Workout GetValidWorkout()
            => new(
                GetValidName(),
                GetValidTeacherId(),
                GetValidStudentId(),
                true
            );

        public string GetValidName() => Faker.Commerce.ProductName();

        public Guid GetValidTeacherId() => Guid.NewGuid();

        public Guid GetValidStudentId() => Guid.NewGuid();
    }
}
