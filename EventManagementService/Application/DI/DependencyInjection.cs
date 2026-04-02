using EventManagementService.Application.Events.Services;
using EventManagementService.Infrastructure.Services;

namespace EventManagementService.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        return services;
    }
    
}