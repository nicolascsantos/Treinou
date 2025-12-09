using MediatR;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Exercise.DeleteExercise
{
    public class DeleteExercise : IDeleteExercise
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitofWork;

        public DeleteExercise(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork)
            => (_exerciseRepository, _unitofWork) = (exerciseRepository, unitOfWork);

        public async Task<Unit> Handle(DeleteExerciseInput request, CancellationToken cancellationToken)
        {
            var exerciseToDelete = await _exerciseRepository.Get(request.Id, cancellationToken);

            if (exerciseToDelete is null) throw new NotFoundException($"Exercise '{request.Id}' not found.");

            await _exerciseRepository.Delete(exerciseToDelete, cancellationToken);
            await _unitofWork.Commit(cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
