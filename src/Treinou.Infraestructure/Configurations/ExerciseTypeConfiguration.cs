using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class ExerciseTypeConfiguration : IEntityTypeConfiguration<ExerciseType>
    {
        public void Configure(EntityTypeBuilder<ExerciseType> exerciseTypeConfiguration)
        {
            exerciseTypeConfiguration.HasKey(x => x.Id);
            exerciseTypeConfiguration.Property(x => x.Name).IsRequired();
        }
    }
}
