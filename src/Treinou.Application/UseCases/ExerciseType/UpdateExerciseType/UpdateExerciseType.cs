using Treinou.Application.UseCases.ExerciseType.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.ExerciseType.UpdateExerciseType
{
    public class UpdateExerciseType : IUpdateExerciseType
    {
        private readonly IExerciseTypeRepository _exerciseTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExerciseType(IExerciseTypeRepository exerciseTypeRepository, IUnitOfWork unitofWork)
            => (_exerciseTypeRepository, _unitOfWork) = (exerciseTypeRepository, unitofWork);
        

        public async Task<ExerciseTypeModelOutput> Handle(UpdateExerciseTypeInput request, CancellationToken cancellationToken)
        {
            var exerciseTypeToBeUpdated = await _exerciseTypeRepository.Get(request.Id, cancellationToken);

            exerciseTypeToBeUpdated.Update(request.Name);

            await _exerciseTypeRepository.Update(exerciseTypeToBeUpdated, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ExerciseTypeModelOutput.FromExerciseType(exerciseTypeToBeUpdated);
        }
    }
}
