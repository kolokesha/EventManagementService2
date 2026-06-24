using EventManagementService.Application.Bookings.Services;
using EventManagementService.Application.Events.Services;
using EventManagementService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagementService.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
    
}