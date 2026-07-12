using Treinou.Domain.Exceptions;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.WorkoutExercise
{
    [Collection(nameof(WorkoutExerciseTestFixture))]
    public class WorkoutExerciseTest
    {
        private readonly WorkoutExerciseTestFixture _fixture;

        public WorkoutExerciseTest(WorkoutExerciseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(InstantiateThrowsWhenOrderIsNotGreaterThanZero))]
        [Trait("Domain", "WorkoutExercise - Aggregates")]
        [InlineData(0)]
        [InlineData(-1)]
        public void InstantiateThrowsWhenOrderIsNotGreaterThanZero(int invalidOrder)
        {
            var action = () => new DomainEntities.WorkoutExercise(
                _fixture.GetValidWorkoutId(),
                _fixture.GetValidExerciseId(),
                invalidOrder,
                _fixture.GetValidNumberOfSets(),
                _fixture.GetValidNumberOfRepetitions(),
                _fixture.GetValidRest(),
                _fixture.GetValidNotes()
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(UpdateThrowsWhenOrderIsZero))]
        [Trait("Domain", "WorkoutExercise - Aggregates")]
        public void UpdateThrowsWhenOrderIsZero()
        {
            var workoutExercise = _fixture.GetValidWorkoutExercise();

            var action = () => workoutExercise.Update(
                0,
                _fixture.GetValidNumberOfSets(),
                _fixture.GetValidNumberOfRepetitions(),
                _fixture.GetValidRest()
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(UpdateThrowsWhenRestIsZero))]
        [Trait("Domain", "WorkoutExercise - Aggregates")]
        public void UpdateThrowsWhenRestIsZero()
        {
            var workoutExercise = _fixture.GetValidWorkoutExercise();

            var action = () => workoutExercise.Update(
                _fixture.GetValidOrder(),
                _fixture.GetValidNumberOfSets(),
                _fixture.GetValidNumberOfRepetitions(),
                TimeSpan.Zero
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(UpdateSucceedsAndMutatesProperties))]
        [Trait("Domain", "WorkoutExercise - Aggregates")]
        public void UpdateSucceedsAndMutatesProperties()
        {
            var workoutExercise = _fixture.GetValidWorkoutExercise();
            var newOrder = _fixture.GetValidOrder();
            var newNumberOfSets = _fixture.GetValidNumberOfSets();
            var newNumberOfRepetitions = _fixture.GetValidNumberOfRepetitions();
            var newRest = TimeSpan.FromSeconds(90);
            var newNotes = _fixture.GetValidNotes();

            workoutExercise.Update(
                newOrder,
                newNumberOfSets,
                newNumberOfRepetitions,
                newRest,
                newNotes
            );

            Assert.Equal(newOrder, workoutExercise.Order);
            Assert.Equal(newNumberOfSets, workoutExercise.NumberOfSets);
            Assert.Equal(newNumberOfRepetitions, workoutExercise.NumberOfRepetitions);
            Assert.Equal(newRest, workoutExercise.Rest);
            Assert.Equal(newNotes, workoutExercise.Notes);
        }
    }
}
