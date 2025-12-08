using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutExercise> workoutExerciseConfiguration)
        {
            workoutExerciseConfiguration
                .HasKey(x => x.Id);

            // FK para Exercise
            workoutExerciseConfiguration
                .Property(x => x.ExerciseId)
                .IsRequired();

            workoutExerciseConfiguration
                .HasOne(x => x.Exercise)
                .WithMany() // se Exercise tiver ICollection<WorkoutExercise>, pode colocar .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Propriedades obrigatórias
            workoutExerciseConfiguration
                .Property(x => x.Order)
                .IsRequired();

            workoutExerciseConfiguration
                .Property(x => x.NumberOfSets)
                .IsRequired();

            workoutExerciseConfiguration
                .Property(x => x.NumberOfRepetitions)
                .IsRequired();

            workoutExerciseConfiguration
                .Property(x => x.Rest)
                .IsRequired();

            // Notes opcional, mas com limite de tamanho
            workoutExerciseConfiguration
                .Property(x => x.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

        }
    }
}
