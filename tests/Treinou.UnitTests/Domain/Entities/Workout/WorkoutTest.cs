using Treinou.Domain.Exceptions;

namespace Treinou.UnitTests.Domain.Entities.Workout
{
    [Collection(nameof(WorkoutTestFixture))]
    public class WorkoutTest
    {
        private readonly WorkoutTestFixture _fixture;

        public WorkoutTest(WorkoutTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(UpdateThrowsWhenNameIsEmpty))]
        [Trait("Domain", "Workout - Aggregates")]
        public void UpdateThrowsWhenNameIsEmpty()
        {
            var workout = _fixture.GetValidWorkout();

            var action = () => workout.Update(
                "",
                _fixture.GetValidTeacherId(),
                _fixture.GetValidStudentId()
            );

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(UpdateSucceedsAndMutatesProperties))]
        [Trait("Domain", "Workout - Aggregates")]
        public void UpdateSucceedsAndMutatesProperties()
        {
            var workout = _fixture.GetValidWorkout();
            var newName = _fixture.GetValidName();
            var newTeacherId = _fixture.GetValidTeacherId();
            var newStudentId = _fixture.GetValidStudentId();

            workout.Update(newName, newTeacherId, newStudentId);

            Assert.Equal(newName, workout.Name);
            Assert.Equal(newTeacherId, workout.TeacherId);
            Assert.Equal(newStudentId, workout.StudentId);
        }
    }
}
