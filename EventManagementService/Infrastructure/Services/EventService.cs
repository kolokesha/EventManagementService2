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
        return events.Select(MapToDto);
    }

    public EventDto? GetEventById(int id)
    {
        var entity = eventRepository.GetById(id);
        return entity == null ? null : MapToDto(entity);
    }

    public EventDto CreateEvent(EventModel eventModel)
    {
        Validate(eventModel);

        var created = eventRepository.Add(eventModel);
        return MapToDto(created);
    }

    public EventDto? UpdateEvent(EventModel eventModel, int eventId)
    {
        Validate(eventModel);

        var updated = eventRepository.Update(eventModel, eventId);
        return updated == null ? null : MapToDto(updated);
    }

    public bool DeleteEventById(int id)
    {
        var existing = eventRepository.GetById(id);
        if (existing == null) return false;

        eventRepository.Delete(id);
        return true;
    }

    private static EventDto MapToDto(EventModel x)
    {
        return new EventDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            StartAt = x.StartAt,
            EndAt = x.EndAt,
        };
    }

    private static void Validate(EventModel model)
    {
        if (model.EndAt < model.StartAt)
            throw new ValidationException("Дата окончания не может быть раньше даты начала");
    }
}