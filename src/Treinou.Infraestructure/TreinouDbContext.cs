using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Infraestructure.Configurations;

namespace Treinou.Infraestructure
{
    public class TreinouDbContext : DbContext
    {
        public DbSet<Student> Students => Set<Student>();

        public DbSet<Teacher> Teachers => Set<Teacher>();

        public DbSet<ExerciseType> ExerciseTypes => Set<ExerciseType>();

        public DbSet<Workout> Workouts { get; set; }

        public TreinouDbContext(DbContextOptions<TreinouDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new ExerciseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutConfiguration());
        }
    }
}
