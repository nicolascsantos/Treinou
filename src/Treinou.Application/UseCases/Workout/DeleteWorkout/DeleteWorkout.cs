using MediatR;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Workout.DeleteWorkout
{
    public class DeleteWorkout : IDeleteWorkout
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWorkout(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
            => (_workoutRepository, _unitOfWork) = (workoutRepository, unitOfWork);


        public async Task<Unit> Handle(DeleteWorkoutInput request, CancellationToken cancellationToken)
        {
            var workoutToBeDeleted = await _workoutRepository.Get(request.Id, cancellationToken);

            if (workoutToBeDeleted is null) 
                throw new NotFoundException($"Workout '{request.Id}' not found.");

            await _workoutRepository.Delete(workoutToBeDeleted, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
