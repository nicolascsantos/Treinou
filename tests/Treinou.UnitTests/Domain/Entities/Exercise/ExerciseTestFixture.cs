using Treinou.UnitTests.Common;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.Exercise
{
    [CollectionDefinition(nameof(ExerciseTestFixture))]
    public class ExerciseTestFixtureCollection : ICollectionFixture<ExerciseTestFixture> { }

    public class ExerciseTestFixture : BaseFixture
    {
        public DomainEntities.Exercise GetValidExercise()
            => new(
                GetValidName(),
                GetValidExerciseTypeId(),
                true,
                GetValidImageUrl()
            );

        public string GetValidName() => Faker.Commerce.ProductName();

        public Guid GetValidExerciseTypeId() => Guid.NewGuid();

        public string GetValidImageUrl() => Faker.Image.PicsumUrl();
    }
}
