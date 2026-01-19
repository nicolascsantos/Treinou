
using Treinou.Application.DTOs;

namespace Treinou.Application.UseCases.Workout.Common
{
    public class WorkoutModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public TeacherDTO Teacher { get; set; }

        public StudentDTO Student { get; set; }

        public bool IsActive { get; set; }

        public WorkoutModelOutput(
            Guid id,
            string name,
            TeacherDTO teacher,
            StudentDTO student,
            bool isActive
        )
        {
            Id = id;
            Name = name;
            Teacher = teacher;
            Student = student;
            IsActive = isActive;
        }

        public static WorkoutModelOutput FromWorkout(Domain.Entities.Workout workout)
        {
            return new WorkoutModelOutput(
                workout.Id,
                workout.Name,
                TeacherDTO.FromTeacher(workout.Teacher),
                StudentDTO.FromStudent(workout.Student),
                workout.IsActive
            );
        }
    }
}
