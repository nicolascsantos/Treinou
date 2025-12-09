using MediatR;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.ExerciseType.DeleteExerciseType
{
    public class DeleteExerciseType : IDeleteExerciseType
    {
        private readonly IExerciseTypeRepository _exerciseTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExerciseType(IExerciseTypeRepository exerciseTypeRepository, IUnitOfWork unitOfWork)
            => (_exerciseTypeRepository, _unitOfWork) = (exerciseTypeRepository, unitOfWork);

        public async Task<Unit> Handle(DeleteExerciseTypeInput request, CancellationToken cancellationToken)
        {
            var exeriseTypeToBeDeleted = 
                await _exerciseTypeRepository.Get(request.Id, cancellationToken);

            if (exeriseTypeToBeDeleted is null) 
                throw new NotFoundException($"Exercise type '{request.Id}' not found.");

            await _exerciseTypeRepository.Delete(exeriseTypeToBeDeleted, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
