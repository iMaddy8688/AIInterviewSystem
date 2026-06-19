using AIInterview.API.Services.SecurityService;
using AIInterview.API.Services.StudentService;
using AIInterview.API.Repostiory.Security;
using AIInterview.API.Repostiory.StudentRepository;

namespace AIInterview.API.ServiceConfig
{
    public static class DIConfig
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Add your service registrations here
            // Example: services.AddScoped<IMyService, MyService>();

            services.AddScoped<IStudentRegistration, StudentRegistrationRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
