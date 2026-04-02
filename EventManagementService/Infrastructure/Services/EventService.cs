using System.ComponentModel.DataAnnotations;
using EventManagementService.Application.Events;
using EventManagementService.Application.Events.Dto;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Infrastructure.Services;

public class EventService(IEventRepository<EventModel> eventRepository) : IEventService
{
    public IEnumerable<EventDto> GetAllEvents()
    {
        var events = eventRepository.GetAll();
        
        var result = events.Select(x => new EventDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            EndAt = x.EndAt,
            StartAt = x.StartAt,
        });
        
        return result;
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
            Title = result.Title,
            Description = result.Description,
            StartAt = result.StartAt,
            EndAt = result.EndAt,
        };
        
    }

    public CreateEventDto UpdateEvent(EventModel eventModel, int eventId)
    {
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