using MediatR;
using Treinou.Application.UseCases.Workout.Common;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.Workout.UpdateWorkout
{
    public class UpdateWorkoutInput : IRequest<WorkoutModelOutput>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Guid TeacherId { get; private set; }

        public Guid StudentId { get; private set; }

        public List<Entity.WorkoutExercise> Exercises { get; private set; }

        public UpdateWorkoutInput(
            Guid id,
            string name,
            Guid teacherId,
            Guid studentId,
            List<Entity.WorkoutExercise> exercises)
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
            StudentId = studentId;
            Exercises = exercises;
        }
    }
}
