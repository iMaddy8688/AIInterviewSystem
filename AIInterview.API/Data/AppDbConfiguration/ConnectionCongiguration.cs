using AIInterview.API.Data;
using Microsoft.EntityFrameworkCore;

public static class ConnectionConfiguration
{
    public static IServiceCollection ConnectionConfig(
        this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString =
            config.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}