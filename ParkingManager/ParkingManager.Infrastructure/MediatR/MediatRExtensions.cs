using System.Reflection;

namespace ParkingManager.ParkingManager.Infrastructure;

public static class MediatRExtensions
{
    public static IServiceCollection  ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}