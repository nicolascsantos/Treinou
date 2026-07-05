using Treinou.Application.UseCases.Exercise.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Exercise.CreateExercise
{
    public class CreateExercise : ICreateExercise
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExercise(
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork
        )
        {
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseModelOutput> Handle(
            CreateExerciseInput request,
            CancellationToken cancellationToken
        )
        {
            var exercise = new Domain.Entities.Exercise(
                request.Name,
                request.ExerciseTypeId,
                request.IsActive,
                request.ImageUrl
            );

            await _exerciseRepository.Insert(exercise, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ExerciseModelOutput.FromExercise(exercise);
        }
    }
}
