using System.Collections.Concurrent;
using EventManagementService.Application.Bookings.Services;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Infrastructure.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public Booking Add(Booking booking)
    {
        _bookings[booking.Id] = booking;
        return booking;
    }

    public Booking? GetById(Guid id)
    {
        _bookings.TryGetValue(id, out var booking);
        return booking;
    }

    public IReadOnlyCollection<Booking> GetPending()
    {
        return _bookings.Values
            .Where(x => x.Status == BookingStatus.Pending)
            .ToList();
    }

    public Booking Update(Booking booking)
    {
        _bookings[booking.Id] = booking;
        return booking;
    }
}