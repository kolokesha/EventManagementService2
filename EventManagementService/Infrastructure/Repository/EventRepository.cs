using EventManagementService.Application.Events.Services;
using EventManagementService.Domain;
using EventManagementService.Domain.Entities;

namespace EventManagementService.Infrastructure.Repository;

public class EventRepository : IEventRepository
{
    private readonly Dictionary<Guid, EventModel> _events = new();
    
    public (List<EventModel> Items, int TotalCount) GetAll(int page = 1, int pageSize = 10, string? title = null, DateTime? from = null, DateTime? to = null)
    {
        var query = _events.Values.AsEnumerable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }
        
        if (from.HasValue)
        {
            query = query.Where(x => x.StartAt >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.EndAt <= to.Value);
        }

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return (items, totalCount);
    }

    public EventModel? GetById(Guid id)
    {
        _events.TryGetValue(id, out var entity);
        return entity;
    }

    public EventModel Add(EventModel eventModel)
    {
        var newId = Guid.NewGuid();
        
        eventModel.Id = newId;
        _events.TryAdd(newId, eventModel);
        
        return eventModel;
    }

    public EventModel? Update(EventModel eventModel, Guid id)
    {
        if (!_events.TryGetValue(id, out var entity))
            return null;

        eventModel.Id = id;
        _events[id] = eventModel;
        return eventModel;

    }

    public bool Delete(Guid id)
    {
        _events.Remove(id);
        return true;
    }
}