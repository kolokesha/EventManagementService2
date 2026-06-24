using EventManagementService.Application.Bookings.Dto;
using EventManagementService.Application.Bookings.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementService.Controllers;

[ApiController]
[Route("bookings")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    [HttpPost("/events/{id:guid}/book")]
    [ProducesResponseType(typeof(BookingInfo), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingInfo>> CreateBooking(Guid id)
    {
        var booking = await bookingService.CreateBookingAsync(id);

        return AcceptedAtRoute(
            "GetBookingById",
            new { id = booking.Id },
            booking);
    }

    [HttpGet("{id:guid}", Name = "GetBookingById")]
    [ProducesResponseType(typeof(BookingInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingInfo>> GetBookingById(Guid id)
    {
        var booking = await bookingService.GetBookingByIdAsync(id);
        return Ok(booking);
    }
}