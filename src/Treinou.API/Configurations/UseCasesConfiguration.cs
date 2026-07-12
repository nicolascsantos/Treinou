using Treinou.Application.Services;
using Treinou.Application.UseCases.Student.CreateStudent;
using Treinou.API.Services;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;
using Treinou.Infraestructure;
using Treinou.Infraestructure.Repositories;

namespace Treinou.API.Configurations
{
    public static class UseCasesConfiguration
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(CreateStudent).Assembly));
            services.AddRepositories();
            services.AddScoped<ITokenService, JwtTokenService>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IExerciseRepository, ExerciseRepository>();
            services.AddTransient<IExerciseTypeRepository, ExerciseTypeRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ITeacherRepository, TeacherRepository>();
            services.AddTransient<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
            services.AddTransient<IWorkoutRepository, WorkoutRepository>();
            services.AddTransient<IWeightEntryRepository, WeightEntryRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
