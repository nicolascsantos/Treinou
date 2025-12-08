using MediatR;
using Treinou.Application.UseCases.Workout.Common;

namespace Treinou.Application.UseCases.Workout.CreateWorkout
{
    public class CreateWorkoutInput : IRequest<WorkoutModelOutput>
    {
        public string Name { get; set; }

        public Guid TeacherId { get; set; }

        public Guid StudentId { get; set; }

        public bool IsActive { get; set; } = true;

        public CreateWorkoutInput(
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
    }
}
