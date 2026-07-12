using Treinou.UnitTests.Common;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.WeightEntry
{
    [CollectionDefinition(nameof(WeightEntryTestFixture))]
    public class WeightEntryTestFixtureCollection : ICollectionFixture<WeightEntryTestFixture> { }

    public class WeightEntryTestFixture : BaseFixture
    {
        public DomainEntities.WeightEntry GetValidWeightEntry()
            => new(
                GetValidStudentId(),
                GetValidWeight(),
                GetValidDateAdded()
            );

        public Guid GetValidStudentId() => Guid.NewGuid();

        public double GetValidWeight() => Math.Round(Faker.Random.Double(40, 150), 1);

        public DateTime GetValidDateAdded() => DateTime.UtcNow.AddDays(-Faker.Random.Int(0, 30));
    }
}
