using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> workoutConfiguration)
        {
            workoutConfiguration.HasKey(x => x.Id);
            workoutConfiguration.Property(x => x.Name)
                .IsRequired();

            workoutConfiguration
                .HasOne(x => x.Teacher)
                .WithMany(t => t.Workouts)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            
            workoutConfiguration
                .HasOne(x => x.Student)
                .WithMany(s => s.Workouts)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            workoutConfiguration
                .HasMany(x => x.Exercises) 
                .WithOne("Workout")                             
                .HasForeignKey("WorkoutId")
                .OnDelete(DeleteBehavior.Cascade);

            workoutConfiguration.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
