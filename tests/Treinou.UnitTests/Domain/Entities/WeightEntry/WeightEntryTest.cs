using Treinou.Domain.Exceptions;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.WeightEntry
{
    [Collection(nameof(WeightEntryTestFixture))]
    public class WeightEntryTest
    {
        private readonly WeightEntryTestFixture _fixture;

        public WeightEntryTest(WeightEntryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(Instantiate_ShouldThrow_WhenWeightIsZeroOrNegative))]
        [Trait("Domain", "WeightEntry - Aggregates")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-70.5)]
        public void Instantiate_ShouldThrow_WhenWeightIsZeroOrNegative(double invalidWeight)
        {
            var action = () => new DomainEntities.WeightEntry(
                _fixture.GetValidStudentId(),
                invalidWeight,
                _fixture.GetValidDateAdded()
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(Instantiate_ShouldSucceed_WithValidWeight))]
        [Trait("Domain", "WeightEntry - Aggregates")]
        public void Instantiate_ShouldSucceed_WithValidWeight()
        {
            var studentId = _fixture.GetValidStudentId();
            var dateAdded = _fixture.GetValidDateAdded();

            var weightEntry = new DomainEntities.WeightEntry(
                studentId,
                70.5,
                dateAdded
            );

            Assert.Equal(studentId, weightEntry.StudentId);
            Assert.Equal(70.5, weightEntry.Weight);
            Assert.Equal(dateAdded, weightEntry.DateAdded);
        }

        [Fact(DisplayName = nameof(Instantiate_ShouldThrow_WhenDateAddedIsInTheFuture))]
        [Trait("Domain", "WeightEntry - Aggregates")]
        public void Instantiate_ShouldThrow_WhenDateAddedIsInTheFuture()
        {
            var futureDate = DateTime.UtcNow.AddDays(1);

            var action = () => new DomainEntities.WeightEntry(
                _fixture.GetValidStudentId(),
                _fixture.GetValidWeight(),
                futureDate
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(Instantiate_ShouldHonorSuppliedDateAdded))]
        [Trait("Domain", "WeightEntry - Aggregates")]
        public void Instantiate_ShouldHonorSuppliedDateAdded()
        {
            var suppliedDate = DateTime.UtcNow.AddDays(-10);

            var weightEntry = new DomainEntities.WeightEntry(
                _fixture.GetValidStudentId(),
                _fixture.GetValidWeight(),
                suppliedDate
            );

            Assert.Equal(suppliedDate, weightEntry.DateAdded);
        }
    }
}
