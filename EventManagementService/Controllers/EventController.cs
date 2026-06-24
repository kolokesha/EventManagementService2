using EventManagementService.Application.Bookings.Dto;
using EventManagementService.Application.Bookings.Services;
using EventManagementService.Application.Common;
using EventManagementService.Application.Events.Dto;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementService.Controllers;

[ApiController]
[Route("events")]
public class EventController(
    IEventService eventService,
    IBookingService bookingService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
    public ActionResult<PaginatedResult<EventDto>> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null)
    {
        var events = eventService.GetAllEvents(page, pageSize, title, from, to);
        return Ok(events);
    }

    [HttpGet("{id:guid}", Name = "GetEventById")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EventDto> GetEventById(Guid id)
    {
        var eventDto = eventService.GetEventById(id);
        return Ok(eventDto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EventDto> CreateEvent([FromBody] CreateEventDto request)
    {
        var eventModel = MapToModel(request);
        var createdEvent = eventService.CreateEvent(eventModel);

        return CreatedAtRoute(
            "GetEventById",
            new { id = createdEvent.Id },
            createdEvent);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EventDto> UpdateEvent(Guid id, [FromBody] CreateEventDto request)
    {
        var eventModel = MapToModel(request);
        var updatedEvent = eventService.UpdateEvent(eventModel, id);

        return Ok(updatedEvent);
    }

    [HttpDelete("{id:guid}", Name = "DeleteEventById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteEventById(Guid id)
    {
        eventService.DeleteEventById(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/book")]
    [ProducesResponseType(typeof(BookingInfo), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingInfo>> CreateBooking(Guid id)
    {
        var booking = await bookingService.CreateBookingAsync(id);

        return AcceptedAtRoute(
            "GetBookingById",
            new { id = booking.Id },
            booking);
    }

    private static EventModel MapToModel(CreateEventDto request)
    {
        return new EventModel
        {
            Title = request.Title,
            Description = request.Description,
            StartAt = (DateTime)request.StartAt!,
            EndAt = (DateTime)request.EndAt!
        };
    }
}