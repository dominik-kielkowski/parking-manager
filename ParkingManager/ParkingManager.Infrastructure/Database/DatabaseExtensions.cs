using Microsoft.EntityFrameworkCore;

namespace parking_manager;

public static class DatabaseExtensions
{
    public static IServiceCollection AddSqliteDatabase<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString("Sqlite");

        services.AddDbContext<TContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}