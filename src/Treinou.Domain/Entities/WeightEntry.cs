using Treinou.Domain.Exceptions;
using Treinou.Domain.SeedWork;

namespace Treinou.Domain.Entities
{
    public class WeightEntry : AggregateRoot
    {
        public Student Student { get; set; }

        public Guid StudentId { get; private set; }

        public double Weight { get; private set; }

        public DateTime DateAdded { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public WeightEntry() {}

        public WeightEntry(Guid studentId, double weight)
        {
            StudentId = studentId;
            Weight = weight;
            DateAdded = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            Validate();
        }

        public void Validate()
        {
            if (DateAdded > DateTime.UtcNow)
                throw new EntityValidationException("Weight date cannot be a future date.");

            if (Weight <= 0)
                throw new EntityValidationException("Invalid weight.");
        }

        public void Update(double weight, DateTime dateAdded)
        {
            Weight = weight;
            DateAdded = dateAdded;
        }
    }
}
