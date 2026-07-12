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
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IExerciseTypeRepository, ExerciseTypeRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IWeightEntryRepository, WeightEntryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
