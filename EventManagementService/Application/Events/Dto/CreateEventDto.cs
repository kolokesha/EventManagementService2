using System.ComponentModel.DataAnnotations;

namespace EventManagementService.Application.Events.Dto;

public class CreateEventDto
{
    [Required(ErrorMessage = "Title обязателен")]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage = "StartAt обязателен")]
    [Range(typeof(DateTime), "2020-01-01", "2030-12-31", ErrorMessage = "Некорректная дата")]
    public DateTime StartAt { get; set; }
    [Required(ErrorMessage = "EndAt обязателен")]
    [Range(typeof(DateTime), "2020-01-01", "2030-12-31", ErrorMessage = "Некорректная дата")]
    public DateTime EndAt { get; set; }
}