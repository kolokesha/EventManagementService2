using EventManagementService.Application.Events.Services;
using EventManagementService.Domain;

namespace EventManagementService.Infrastructure.Repository;

public class EventRepository<T> : IEventRepository<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _events = new();
    private int _idCounter = 0;
    
    public List<T> GetAll()
    {
        return _events.Values.ToList();
    }

    public T? GetById(int id)
    {
        _events.TryGetValue(id, out var entity);
        return entity;
    }

    public T Add(T eventModel)
    {
        _events.TryAdd(_idCounter++, eventModel);
        return eventModel;
    }

    public T? Update(T eventModel, int id)
    {
        if (!_events.TryGetValue(id, out var entity))
            return null;

        _events[id] = eventModel;
        return eventModel;

    }

    public bool Delete(int id)
    {
        _events.Remove(id);
        return true;
    }
}