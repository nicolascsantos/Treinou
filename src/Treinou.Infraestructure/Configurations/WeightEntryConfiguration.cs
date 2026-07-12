using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class WeightEntryConfiguration : IEntityTypeConfiguration<WeightEntry>
    {
        public void Configure(EntityTypeBuilder<WeightEntry> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Weight)
                .IsRequired();

            builder.Property(x => x.DateAdded)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.Student)
                .WithMany(x => x.WeightEntries)
                .HasForeignKey(x => x.StudentId);
        }
    }
}
