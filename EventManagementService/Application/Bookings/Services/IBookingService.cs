using EventManagementService.Application.Bookings.Dto;

namespace EventManagementService.Application.Bookings.Services;

public interface IBookingService
{
    Task<BookingInfo> CreateBookingAsync(Guid eventId);
    Task<BookingInfo> GetBookingByIdAsync(Guid bookingId);
}