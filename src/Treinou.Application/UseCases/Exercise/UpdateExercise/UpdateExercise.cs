using Treinou.Application.UseCases.Exercise.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Exercise.UpdateExercise
{
    public class UpdateExercise : IUpdateExercise
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExercise(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork)
        => (_exerciseRepository, _unitOfWork) = (exerciseRepository, unitOfWork);

        public async Task<ExerciseModelOutput> Handle(UpdateExerciseInput request, CancellationToken cancellationToken)
        {
            var exerciseToBeUpdated = await _exerciseRepository.Get(request.Id, cancellationToken);

            exerciseToBeUpdated.Update(request.Name, request.ExerciseTypeId, null);

            await _exerciseRepository.Update(exerciseToBeUpdated, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return ExerciseModelOutput.FromExercise(exerciseToBeUpdated);
        }
    }
}
