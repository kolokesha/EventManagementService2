using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Events.Services;

public interface IEventRepository
{
    (List<EventModel> Items, int TotalCount) GetAll(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null);
    EventModel? GetById(Guid id);
    EventModel Add(EventModel eventModel);
    EventModel? Update(EventModel eventModel, Guid id);
    bool Delete(Guid id);
}