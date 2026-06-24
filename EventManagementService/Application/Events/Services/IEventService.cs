using EventManagementService.Application.Common;
using EventManagementService.Application.Events.Dto;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Events.Services;

public interface IEventService
{
    public PaginatedResult<EventDto> GetAllEvents(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null);
    public EventDto? GetEventById(Guid id);
    public bool DeleteEventById(Guid id);
    public CreateEventDto CreateEvent(EventModel eventModel);
    
    public CreateEventDto? UpdateEvent(EventModel eventModel, Guid eventId);
}