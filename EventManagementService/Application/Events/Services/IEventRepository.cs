using EventManagementService.Domain.Entities;

namespace EventManagementService.Application.Events.Services;

public interface IEventRepository
{
    List<EventModel> GetAll(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null);
    EventModel? GetById(int id);
    EventModel Add(EventModel eventModel);
    EventModel? Update(EventModel eventModel, int id);
    bool Delete(int id);
}