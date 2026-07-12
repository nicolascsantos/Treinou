using Treinou.Application.UseCases.WeightEntry.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;
using DomainEntity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.WeightEntry.CreateWeightEntry
{
    public class CreateWeightEntry : ICreateWeightEntry
    {
        private readonly IWeightEntryRepository _weightEntryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWeightEntry(IWeightEntryRepository weightEntryRepository, IUnitOfWork unitOfWork)
            => (_weightEntryRepository, _unitOfWork) = (weightEntryRepository, unitOfWork);

        public async Task<WeightEntryModelOutput> Handle(CreateWeightEntryInput request, CancellationToken cancellationToken)
        {
            var weightEntry = new DomainEntity.WeightEntry(
                request.StudentId,
                request.Weight,
                request.DateAdded
            );

            await _weightEntryRepository.Insert(weightEntry, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new WeightEntryModelOutput(
                weightEntry.Id,
                weightEntry.StudentId,
                weightEntry.Weight, 
                weightEntry.DateAdded,
                weightEntry.CreatedAt
            );
        }
    }
}
