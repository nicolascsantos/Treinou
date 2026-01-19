using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Workout.CreateWorkout
{
    public class CreateWorkout : ICreateWorkout
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWorkout(
            IWorkoutRepository workoutRepository,
            ITeacherRepository teacherRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workoutRepository = workoutRepository;
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutModelOutput> Handle(
            CreateWorkoutInput request,
            CancellationToken cancellationToken
        )
        {
            var workout = new Domain.Entities.Workout(
                request.Name,
                request.TeacherId,
                request.StudentId,
                request.IsActive
            );

            if (request.TeacherId != default)
            {
                var teacher = await _teacherRepository.Get(request.TeacherId, cancellationToken);
                NotFoundException.ThrowIfNull(teacher, $"Teacher '{request.TeacherId}' not found ");
                workout.AddTeacher(teacher);
            }

            await _workoutRepository.Insert(workout, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return WorkoutModelOutput.FromWorkout(workout);
        }
    }
}
