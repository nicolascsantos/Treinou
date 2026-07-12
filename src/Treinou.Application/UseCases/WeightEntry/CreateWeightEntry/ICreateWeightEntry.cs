using MediatR;
using Treinou.Application.UseCases.WeightEntry.Common;

namespace Treinou.Application.UseCases.WeightEntry.CreateWeightEntry
{
    public interface ICreateWeightEntry : IRequestHandler<CreateWeightEntryInput, WeightEntryModelOutput>
    {
    }
}
