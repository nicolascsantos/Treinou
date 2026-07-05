using Treinou.Application.UseCases.ExerciseType.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.ExerciseType.CreateExerciseType
{
    public class CreateExerciseType : ICreateExerciseType
    {
        private readonly IExerciseTypeRepository _exerciseTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExerciseType(
            IExerciseTypeRepository exerciseTypeRepository,
            IUnitOfWork unitOfWork
        )
        {
            _exerciseTypeRepository = exerciseTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseTypeModelOutput> Handle(
            CreateExerciseTypeInput request,
            CancellationToken cancellationToken
        )
        {
            var exerciseType = new Domain.Entities.ExerciseType(request.Name);

            await _exerciseTypeRepository.Insert(exerciseType, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ExerciseTypeModelOutput.FromExerciseType(exerciseType);
        }
    }
}
