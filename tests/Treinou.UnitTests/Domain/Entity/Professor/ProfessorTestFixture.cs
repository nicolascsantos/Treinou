using Treinou.UnitTests.Common;

namespace Treinou.UnitTests.Domain.Entity.Professor
{
    [CollectionDefinition(nameof(ProfessorTestFixture))]
    public class ProfessorTestFixtureCollection : ICollectionFixture<ProfessorTestFixture> { }

    public class ProfessorTestFixture : BaseFixture
    {
    }
}
