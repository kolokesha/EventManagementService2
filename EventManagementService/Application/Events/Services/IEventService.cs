using EventManagementService.Application.Events.Dto;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Events.Services;

public interface IEventService
{
    public IEnumerable<EventDto> GetAllEvents();
    public EventDto? GetEventById(int id);
    public bool DeleteEventById(int id);
    public EventDto CreateEvent(EventModel eventModel);
    
    public EventDto? UpdateEvent(EventModel eventModel, int eventId);
}