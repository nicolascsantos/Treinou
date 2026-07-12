using Treinou.Domain.Exceptions;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.Exercise
{
    [Collection(nameof(ExerciseTestFixture))]
    public class ExerciseTest
    {
        private readonly ExerciseTestFixture _fixture;

        public ExerciseTest(ExerciseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(InstantiateActiveByDefault))]
        [Trait("Domain", "Exercise - Aggregates")]
        public void InstantiateActiveByDefault()
        {
            var validExercise = _fixture.GetValidExercise();

            var newExercise = new DomainEntities.Exercise(
                validExercise.Name,
                validExercise.ExcerciseTypeId
            );

            Assert.Equal(validExercise.Name, newExercise.Name);
            Assert.Equal(validExercise.ExcerciseTypeId, newExercise.ExcerciseTypeId);
            Assert.True(newExercise.IsActive);
        }

        [Fact(DisplayName = nameof(InstantiateRespectsIsActiveFalse))]
        [Trait("Domain", "Exercise - Aggregates")]
        public void InstantiateRespectsIsActiveFalse()
        {
            var validExercise = _fixture.GetValidExercise();

            var newExercise = new DomainEntities.Exercise(
                validExercise.Name,
                validExercise.ExcerciseTypeId,
                isActive: false
            );

            Assert.False(newExercise.IsActive);
        }

        [Fact(DisplayName = nameof(InstantiateThrowsWhenNameIsEmpty))]
        [Trait("Domain", "Exercise - Aggregates")]
        public void InstantiateThrowsWhenNameIsEmpty()
        {
            var validExercise = _fixture.GetValidExercise();

            var action = () => new DomainEntities.Exercise(
                "",
                validExercise.ExcerciseTypeId
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(ActivateSetsIsActiveTrue))]
        [Trait("Domain", "Exercise - Aggregates")]
        public void ActivateSetsIsActiveTrue()
        {
            var exercise = _fixture.GetValidExercise();
            exercise.Deactivate();

            exercise.Activate();

            Assert.True(exercise.IsActive);
        }

        [Fact(DisplayName = nameof(DeactivateSetsIsActiveFalse))]
        [Trait("Domain", "Exercise - Aggregates")]
        public void DeactivateSetsIsActiveFalse()
        {
            var exercise = _fixture.GetValidExercise();

            exercise.Deactivate();

            Assert.False(exercise.IsActive);
        }
    }
}
