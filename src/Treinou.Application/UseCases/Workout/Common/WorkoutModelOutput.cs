namespace Treinou.Application.UseCases.Workout.Common
{
    public class WorkoutModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TeacherId { get; set; }

        public Guid StudentId { get; set; }

        public bool IsActive { get; set; }

        public WorkoutModelOutput(
            Guid id,
            string name,
            Guid teacherId,
            Guid studentId,
            bool isActive
        )
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
            StudentId = studentId;
            IsActive = isActive;
        }

        public static WorkoutModelOutput FromWorkout(Domain.Entities.Workout workout)
        {
            return new WorkoutModelOutput(
                workout.Id,
                workout.Name,
                workout.TeacherId,
                workout.StudentId,
                workout.IsActive
            );
        }
    }
}
