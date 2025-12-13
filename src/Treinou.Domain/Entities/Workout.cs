using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class Workout : AggregateRoot
    {
        public string Name { get; private set; }

        public Guid TeacherId { get; private set; }
        public Teacher Teacher { get; private set; }

        public Guid StudentId { get; private set; }
        public Student Student { get; private set; }

        public bool IsActive { get; private set; }

        private readonly List<WorkoutExercise> _exercises = new();
        public IReadOnlyCollection<WorkoutExercise> Exercises => _exercises.AsReadOnly();

        public Workout(
            string name,
            Guid teacherId,
            Guid studentId,
            bool isActive = true
        )
        {
            Name = name;
            TeacherId = teacherId;
            StudentId = studentId;
            IsActive = isActive;
        }

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }

        public void Update(
            string name,
            Guid teacherId,
            Guid studentId
        )
        {
            Name = name;
            TeacherId = teacherId;
            StudentId = studentId; 
        }

        public void AddExercise(
            Guid exerciseId,
            Exercise exercise,
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string notes = ""
        )
        {
            var exerciseAlreadyExists = _exercises.Any(x => x.ExerciseId == exerciseId);

            if (exerciseAlreadyExists)
                throw new InvalidOperationException($"Exercise '{exerciseId}' already exists in this workout.");

            var workoutExercise = new WorkoutExercise(
                exerciseId,
                order,
                numberOfSets,
                numberOfRepetitions,
                rest,
                notes
            );

            _exercises.Add(workoutExercise);
        }

        public void UpdateExercise(
            Guid workoutExerciseId,
            int order,
            int numberOfSets,
            int numberOfRepetitions,
            TimeSpan rest,
            string? notes = null
        )
        {
            var workoutExercise = _exercises.Find(x => x.Id == workoutExerciseId);

            if (workoutExercise is null)
                throw new InvalidOperationException($"WorkoutExercise '{workoutExerciseId}' not found in this workout.");

            workoutExercise.Update(order, numberOfSets, numberOfRepetitions, rest, notes);
        }

        public void RemoveExercise(Guid exerciseId)
        {
            var exerciseToRemove = _exercises.Find(x => x.Id == exerciseId);

            if (exerciseToRemove is null) return;

            _exercises.Remove(exerciseToRemove);
        }
    }
}
