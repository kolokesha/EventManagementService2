using System.Text.Json.Serialization;

namespace EventManagementService.Domain.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookingStatus
{
    Pending,
    Confirmed,
    Rejected
}