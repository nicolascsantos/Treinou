using MediatR;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.WorkoutExercise.DeleteWorkoutExercise
{
    public class DeleteWorkoutExercise : IDeleteWorkoutExercise
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWorkoutExercise(IWorkoutExerciseRepository workoutExerciseRepository, IUnitOfWork unitOfWork)
            => (_workoutExerciseRepository, _unitOfWork) = (workoutExerciseRepository, unitOfWork);

        public async Task<Unit> Handle(DeleteWorkoutExerciseInput request, CancellationToken cancellationToken)
        {
            var workoutExerciseToBeDeleted = await _workoutExerciseRepository.Get(request.Id, cancellationToken);

            await _workoutExerciseRepository.Delete(workoutExerciseToBeDeleted, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return await Task.FromResult(Unit.Value);
        }
    }
}
