using EventManagementService.Application.Bookings.Services;
using EventManagementService.Application.Events.Services;
using EventManagementService.Infrastructure.Repository;
using EventManagementService.Infrastructure.Services;

namespace EventManagementService.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IEventRepository, EventRepository>();
        services.AddSingleton<IBookingRepository, BookingRepository>();
        services.AddHostedService<BookingProcessingService>();
        return services;
    }
}