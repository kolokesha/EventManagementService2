using EventManagementService.Application.Bookings.Dto;
using EventManagementService.Application.Bookings.Services;
using EventManagementService.Application.Events.Dto.Common;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Infrastructure.Services;

public class BookingService(
    IBookingRepository bookingRepository,
    IEventRepository eventRepository) : IBookingService
{
    public Task<BookingInfo> CreateBookingAsync(Guid eventId)
    {
        var eventModel = eventRepository.GetById(eventId);
        if (eventModel == null)
            throw new NotFoundException("Event not found");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdBooking = bookingRepository.Add(booking);
        return Task.FromResult(MapToInfo(createdBooking));
    }

    public Task<BookingInfo> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = bookingRepository.GetById(bookingId);
        if (booking == null)
            throw new NotFoundException("Booking not found");

        return Task.FromResult(MapToInfo(booking));
    }

    private static BookingInfo MapToInfo(Booking booking)
    {
        return new BookingInfo
        {
            Id = booking.Id,
            EventId = booking.EventId,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            ProcessedAt = booking.ProcessedAt
        };
    }
}