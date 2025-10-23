using FluentAssertions;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Coach
{
    [Collection(nameof(CoachTestFixure))]
    public class CoachTest
    {
        private readonly CoachTestFixure _fixture;

        public CoachTest(CoachTestFixure fixture)
            => _fixture = fixture;


        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Coach - Domain")]
        public void Instantiate()
        {
            // Arrange
            var validCoach = _fixture.GetValidCoach();

            // Act
            var dateBeforeInstantiation = DateTime.Now;
            var coach = new DomainEntities.Coach();
            var dateAfterInstantiation = DateTime.Now;

            // Assert
            coach.Should().NotBeNull();
            coach.Id.Should().NotBeEmpty();
            coach.Name.Should().Be(validCoach.Name);
            coach.CPF.Should().Be(validCoach.Document);
            coach.DateOfBirth.Should().Be(validCoach.DateOfBirth);
            coach.Email.Should().Be(validCoach.Email);
            coach.CREF.Should().Be(validCoach.CREFNumber);
            coach.Phone.Should().Be(validCoach.Phone);
            coach.CreatedAt.Should().NotBeSameDateAs(default);
            coach.CreatedAt.Should().BeAfter(dateBeforeInstantiation).And.BeBefore(dateAfterInstantiation);
        }
    }
}