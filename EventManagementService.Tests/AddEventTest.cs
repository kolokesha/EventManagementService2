using System.ComponentModel.DataAnnotations;
using EventManagementService.Application.Events.Services;
using EventManagementService.Domain.Entities;
using EventManagementService.Infrastructure.Repository;
using EventManagementService.Infrastructure.Services;
using Moq;
using Xunit;

namespace EventManagementService.Tests;

public class AddEventTest
{
    [Fact]
    public void AddEvent_SHouldAddEventOnce()
    {
        var mockRepository = new Mock<IEventRepository>();

        mockRepository
            .Setup(r => r.Add(It.IsAny<EventModel>()))
            .Returns((EventModel e) =>
            {
                e.Id = 1;
                return e;
            });

        var service = new EventService(mockRepository.Object);

        var model = new EventModel
        {
            Title = "Title",
            Description = "Description",
            EndAt = DateTime.Now.AddDays(3),
            StartAt = DateTime.Now,
        };
        
        service.CreateEvent(model);
        
        //Assert
        // Проверка, что метод, настроенный в моке, вызывался один раз
        // Более подробно метод Verify рассмотрим далее в уроке
        mockRepository.Verify(r => r.Add(It.IsAny<EventModel>()), Times.Once);
    }
    
    [Fact]
    public void GetAllEvents_ShouldReturnList()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        service.CreateEvent(new EventModel
        {
            Title = "Title ShouldReturnList",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddDays(1)
        });

        var result = service.GetAllEvents();

        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public void GetById_ShouldReturnEvent()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        var created = service.CreateEvent(new EventModel
        {
            Title = "Test",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });

        var result = service.GetEventById(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
    }
    
    [Fact]
    public void UpdateEvent_ShouldChangeData()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        var oldModel = new EventModel
        {
            Title = "Old",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        };

        var created = service.CreateEvent(oldModel);

        var newModel = new EventModel
        {
            Title = "New",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        };

        var updated = service.UpdateEvent(newModel, created.Id);

        Assert.Equal("New", updated.Title);
    }
    
    [Fact]
    public void DeleteEvent_ShouldRemoveEvent()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        var created = service.CreateEvent(new EventModel
        {
            Title = "Test",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });

        var deleted = service.DeleteEventById(created.Id);

        Assert.True(deleted);
        Assert.Null(service.GetEventById(created.Id));
    }
    
    [Fact]
    public void Filter_ByTitle_ShouldReturnCorrectEvents()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        service.CreateEvent(new EventModel
        { 
            Title = "Haski",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });
        service.CreateEvent(new EventModel
        {
            Title = "Noize",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });

        var result = service.GetAllEvents(title: "Haski");

        Assert.Single(result.Items);
        Assert.Equal("Haski", result.Items.First().Title);
    }
    
    [Fact]
    public void Filter_ByDate_ShouldReturnCorrectEvents()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        service.CreateEvent(new EventModel
        {
            Title = "Old",
            StartAt = DateTime.Now.AddDays(-10),
            EndAt = DateTime.Now.AddDays(-9)
        });

        service.CreateEvent(new EventModel
        {
            Title = "New",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });

        var result = service.GetAllEvents(from: DateTime.Now.AddDays(-1));

        Assert.Single(result.Items);
    }
    
    [Fact]
    public void Pagination_ShouldReturnCorrectPageSize()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        for (int i = 0; i < 20; i++)
        {
            service.CreateEvent(new EventModel
            {
                Title = $"Event {i}",
                StartAt = DateTime.Now,
                EndAt = DateTime.Now.AddHours(1)
            });
        }

        var result = service.GetAllEvents(page: 2, pageSize: 5);

        Assert.Equal(5, result.TotalCount);
    }
    
    [Fact]
    public void CombinedFilter_ShouldReturnCorrectResult()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        service.CreateEvent(new EventModel
        {
            Title = "Music",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        });

        service.CreateEvent(new EventModel
        {
            Title = "Sport",
            StartAt = DateTime.Now.AddDays(2),
            EndAt = DateTime.Now.AddDays(2).AddHours(1)
        });

        var result = service.GetAllEvents(
            title: "Music",
            from: DateTime.Now.AddDays(-1),
            to: DateTime.Now.AddDays(1));

        Assert.Single(result.Items);
    }
    
    [Fact]
    public void GetById_NonExisting_ShouldReturnNull()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        var result = service.GetEventById(999);

        Assert.Null(result);
    }
    
    [Fact]
    public void Create_InvalidDates_ShouldThrow()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);

        var model = new EventModel
        {
            Title = "Test",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddDays(-1)
        };

        Assert.Throws<ValidationException>(() => service.CreateEvent(model));
    }
    
    [Fact]
    public void Update_InvalidDates_ShouldThrow()
    {
        var repo = CreateRepo();
        var service = new EventService(repo);
        
        var model = new EventModel
        {
            Title = "Test",
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddHours(1)
        };

        var created = service.CreateEvent(model);
        
        model.EndAt = DateTime.Now.AddDays(-1);
        
        Assert.Throws<ValidationException>(() =>
            service.UpdateEvent(model, created.Id));
    }
    
    
    private EventRepository CreateRepo()
    {
        return new EventRepository();
    }
}