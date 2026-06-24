using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Bookings.Services;

public interface IBookingRepository
{
    Booking Add(Booking booking);
    Booking? GetById(Guid id);
    IReadOnlyCollection<Booking> GetPending();
    Booking Update(Booking booking);
}