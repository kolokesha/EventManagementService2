using EventManagementService.Application.Common;
using EventManagementService.Application.Events.Dto;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Events.Services;

public interface IEventService
{
    public PaginatedResult<EventDto> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null);
    public EventDto? GetEventById(int id);
    public bool DeleteEventById(int id);
    public EventDto CreateEvent(EventModel eventModel);
    
    public EventDto? UpdateEvent(EventModel eventModel, int eventId);
}