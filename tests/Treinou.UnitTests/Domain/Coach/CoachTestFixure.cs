using Treinou.UnitTests.Common;

namespace Treinou.UnitTests.Domain.Coach
{
    [CollectionDefinition(nameof(CoachTestFixure))]
    public class CoachTestFixureCollection : ICollectionFixture<CoachTestFixure> { }
    public class CoachTestFixure : BaseFixture
    {

    }
}
