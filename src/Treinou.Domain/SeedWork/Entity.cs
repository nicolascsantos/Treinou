namespace Treinou.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
            => Id = Guid.NewGuid();
    }
}
