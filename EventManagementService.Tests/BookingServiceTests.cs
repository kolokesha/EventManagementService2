using EventManagementService.Application.Events.Dto.Common;
using EventManagementService.Domain.Entities;
using EventManagementService.Infrastructure.Repository;
using EventManagementService.Infrastructure.Services;
using Xunit;

namespace EventManagementService.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateBooking_ForExistingEvent_ShouldReturnPendingBookingInfo()
    {
        var (service, _, eventModel) = CreateServiceWithEvent();

        var result = await service.CreateBookingAsync(eventModel.Id);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(eventModel.Id, result.EventId);
        Assert.Equal(BookingStatus.Pending, result.Status);
        Assert.NotEqual(default, result.CreatedAt);
        Assert.Null(result.ProcessedAt);
    }

    [Fact]
    public async Task CreateBooking_MultipleForSameEvent_ShouldCreateUniqueIds()
    {
        var (service, _, eventModel) = CreateServiceWithEvent();

        var first = await service.CreateBookingAsync(eventModel.Id);
        var second = await service.CreateBookingAsync(eventModel.Id);

        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(eventModel.Id, first.EventId);
        Assert.Equal(eventModel.Id, second.EventId);
    }

    [Fact]
    public async Task GetBookingById_ShouldReturnCorrectBookingInfo()
    {
        var (service, _, eventModel) = CreateServiceWithEvent();
        var created = await service.CreateBookingAsync(eventModel.Id);

        var result = await service.GetBookingByIdAsync(created.Id);

        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.EventId, result.EventId);
        Assert.Equal(created.Status, result.Status);
    }

    [Theory]
    [InlineData(BookingStatus.Confirmed)]
    [InlineData(BookingStatus.Rejected)]
    public async Task GetBookingById_ShouldReflectChangedStatus(BookingStatus status)
    {
        var (service, bookingRepository, eventModel) = CreateServiceWithEvent();
        var created = await service.CreateBookingAsync(eventModel.Id);
        var booking = bookingRepository.GetById(created.Id)!;

        booking.Status = status;
        booking.ProcessedAt = DateTime.UtcNow;
        bookingRepository.Update(booking);

        var result = await service.GetBookingByIdAsync(created.Id);

        Assert.Equal(status, result.Status);
        Assert.NotNull(result.ProcessedAt);
    }

    [Fact]
    public async Task CreateBooking_ForNonExistingEvent_ShouldThrowNotFoundException()
    {
        var service = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.CreateBookingAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateBooking_ForDeletedEvent_ShouldThrowNotFoundException()
    {
        var eventRepository = new EventRepository();
        var bookingRepository = new BookingRepository();
        var service = new BookingService(bookingRepository, eventRepository);
        var eventModel = eventRepository.Add(CreateEventModel());

        eventRepository.Delete(eventModel.Id);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.CreateBookingAsync(eventModel.Id));
    }

    [Fact]
    public async Task GetBookingById_ForNonExistingBooking_ShouldThrowNotFoundException()
    {
        var service = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetBookingByIdAsync(Guid.NewGuid()));
    }

    private static BookingService CreateService()
    {
        return new BookingService(new BookingRepository(), new EventRepository());
    }

    private static (BookingService Service, BookingRepository BookingRepository, EventModel EventModel) CreateServiceWithEvent()
    {
        var eventRepository = new EventRepository();
        var bookingRepository = new BookingRepository();
        var service = new BookingService(bookingRepository, eventRepository);
        var eventModel = eventRepository.Add(CreateEventModel());

        return (service, bookingRepository, eventModel);
    }

    private static EventModel CreateEventModel()
    {
        return new EventModel
        {
            Title = "Booking test event",
            StartAt = DateTime.UtcNow.AddDays(1),
            EndAt = DateTime.UtcNow.AddDays(1).AddHours(2)
        };
    }
}
