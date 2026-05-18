using System.ComponentModel.DataAnnotations;
using EventManagementService.Application.Common;
using EventManagementService.Application.Events;
using EventManagementService.Application.Events.Dto;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;

namespace EventManagementService.Infrastructure.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    public PaginatedResult<EventDto> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null)
    {
        var events = eventRepository.GetAll(page,pageSize, title, from, to);
        
        var totalEvents = events.Count;

        var eventDto = events.Select(x => new EventDto()
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            EndAt = x.EndAt,
            StartAt = x.StartAt
        });

        return new PaginatedResult<EventDto>
        {
            TotalCount = totalEvents,
            Page = page,
            PageSize = pageSize,
            Items = eventDto
        };
    }

    public EventDto GetEventById(int id)
    {
        var foundedEvent = eventRepository.GetById(id);
        if (foundedEvent == null)
            return null;

        return new EventDto
        {
            Id = foundedEvent.Id,
            Title = foundedEvent.Title,
            Description = foundedEvent.Description,
            StartAt = foundedEvent.StartAt,
            EndAt = foundedEvent.EndAt,
        };
    }

    public bool DeleteEventById(int id)
    {
        eventRepository.Delete(id);
        return true;
    }

    public CreateEventDto CreateEvent(EventModel eventModel)
    {
        if(eventModel.EndAt < eventModel.StartAt)
        {
            throw new ValidationException("Дата окончания не может быть раньше даты начала");
        }
        
        var result = eventRepository.Add(eventModel);

        return new CreateEventDto()
        {
            Id = result.Id,
            Title = result.Title,
            Description = result.Description,
            StartAt = result.StartAt,
            EndAt = result.EndAt,
        };
        
    }

    public CreateEventDto UpdateEvent(EventModel eventModel, int eventId)
    {
        if(eventModel.EndAt < eventModel.StartAt)
        {
            throw new ValidationException("Дата окончания не может быть раньше даты начала");
        }
        
        var updatedEvent = eventRepository.Update(eventModel, eventId);
        var result = new CreateEventDto();

        if (updatedEvent != null)
        {
            result = new CreateEventDto
            {
                Title = updatedEvent.Title,
                Description = updatedEvent.Description,
                StartAt = updatedEvent.StartAt,
                EndAt = updatedEvent.EndAt,
            };
        }
        
        return result;
    }
}