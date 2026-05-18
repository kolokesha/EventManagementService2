namespace EventManagementService.Application.Events.Dto.Common;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}