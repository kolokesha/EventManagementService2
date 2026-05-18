using EventManagementService.Application.Common;
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
    public PaginatedResult<EventDto> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null)
    {
        return eventService.GetAllEvents(page, pageSize, title,  from, to);
    }
    
    [HttpGet("{id}", Name = "GetEventById")]
    public IActionResult GetEventById(int id)
    {
        var foundedEvent = eventService.GetEventById(id);
        if (foundedEvent == null)
        {
            return NotFound();
        }
        return Ok(foundedEvent);
    }

    [HttpPost]
    public CreateEventDto CreateEvent([FromBody] CreateEventDto request)
    {
        var eventModel = new EventModel
        {
            Title = request.Title,
            Description = request.Description,
            StartAt = request.StartAt,
            EndAt = request.EndAt
        };
        
        var createdEvent = eventService.CreateEvent(eventModel);
        
        return createdEvent;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEvent(int id, [FromBody] CreateEventDto request)
    {
        var foundedEvent = eventService.GetEventById(id);
        
        if (foundedEvent == null)
        {
            return NotFound();
        }
        var eventModel = new EventModel()
        {
            Title = request.Title,
            Description = request.Description,
            StartAt = request.StartAt,
            EndAt = request.EndAt
        };
        
        var result = eventService.UpdateEvent(eventModel, id);
        
        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
        
    }

    [HttpDelete("{id}", Name = "DeleteEventById")]
    public IActionResult DeleteEventById(int id)
    {
        var foundedEvent = eventService.GetEventById(id);
        if (foundedEvent == null)
        {
            return NotFound();
        }
        eventService.DeleteEventById(id);
        return Ok();
    }
}