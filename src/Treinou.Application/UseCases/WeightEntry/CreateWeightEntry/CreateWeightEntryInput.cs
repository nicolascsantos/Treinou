using MediatR;
using Treinou.Application.UseCases.WeightEntry.Common;

namespace Treinou.Application.UseCases.WeightEntry.CreateWeightEntry
{
    public class CreateWeightEntryInput : IRequest<WeightEntryModelOutput>
    {
        public Guid StudentId { get; private set; }

        public double Weight { get; private set; }

        public DateTime DateAdded { get; private set; }

        public CreateWeightEntryInput(
            Guid studentId,
            double weight,
            DateTime dateAdded
        )
        {
            StudentId = studentId;
            Weight = weight;
            DateAdded = dateAdded;
        }
    }
}
