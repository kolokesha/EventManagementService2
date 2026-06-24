using EventManagementService.Application.Bookings.Services;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Infrastructure.Services;

public class BookingProcessingService(
    IBookingRepository bookingRepository,
    ILogger<BookingProcessingService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pendingBookings = bookingRepository.GetPending();

                foreach (var booking in pendingBookings)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

                    booking.Status = BookingStatus.Confirmed;
                    booking.ProcessedAt = DateTime.UtcNow;
                    bookingRepository.Update(booking);

                    logger.LogInformation(
                        "Booking {BookingId} for event {EventId} was confirmed",
                        booking.Id,
                        booking.EventId);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("Booking processing service is stopping");
        }
    }
}
