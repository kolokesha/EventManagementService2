using EventManagementService.Application.Events.Dto;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
    public ActionResult<PaginatedResult<EventDto>> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null)
    {
        var events = eventService.GetAllEvents(page, pageSize, title,  from, to);
        return Ok(events);
    }

    [HttpGet("{id}", Name = "GetEventById")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EventDto> GetEventById(int id)
    {
        var eventDto = eventService.GetEventById(id);
        return eventDto == null ? NotFound() : Ok(eventDto);
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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EventDto> UpdateEvent(int id, [FromBody] CreateEventDto request)
    {
        var eventModel = MapToModel(request);
        var updatedEvent = eventService.UpdateEvent(eventModel, id);

        return updatedEvent == null ? NotFound() : Ok(updatedEvent);
    }

    [HttpDelete("{id}", Name = "DeleteEventById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteEventById(int id)
    {
        var deleted = eventService.DeleteEventById(id);
        return deleted ? NoContent() : NotFound();
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