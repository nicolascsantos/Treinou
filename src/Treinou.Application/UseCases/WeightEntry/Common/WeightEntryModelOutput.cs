namespace Treinou.Application.UseCases.WeightEntry.Common
{
    public class WeightEntryModelOutput
    {
        public Guid Id { get; private set; }

        public Guid StudentId { get; private set; }

        public double Weight { get; private set; }

        public DateTime DateAdded { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public WeightEntryModelOutput(
            Guid id,
            Guid studentId,
            double weight,
            DateTime dateAdded,
            DateTime createdAt
        )
        {
            Id = id;
            StudentId = studentId;
            Weight = weight;
            DateAdded = dateAdded;
            CreatedAt = createdAt;
        }
    }
}
