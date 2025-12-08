using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> exerciseConfiguration)
        {
            exerciseConfiguration.HasKey(x => x.Id);
            exerciseConfiguration
                .Property(x => x.Name)
                .IsRequired();
            exerciseConfiguration
                .Property(x => x.ImageUrl)
                .IsRequired(false);
            exerciseConfiguration
                .HasOne(x => x.ExcerciseType)
                .WithMany(t => t.Exercises)
                .HasForeignKey(x => x.ExcerciseTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
