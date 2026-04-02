namespace EventManagementService.Application.Events.Services;

public interface IEventRepository<T>
{
    List<T> GetAll();
    T? GetById(int id);
    T Add(T eventModel);
    T? Update(T eventModel, int id);
    bool Delete(int id);
}