using EventBooking.Api.Data;
using EventBooking.Api.Dtos;
using EventBooking.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using EventBooking.Api.Services;
using System.Diagnostics;

namespace EventBooking.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _bookingService.CreateBookingAsync(userId, request.SeatId);
        switch (result)
        {
            case BookingResult.SeatNotFound:
                return NotFound("Seat not found.");
            case BookingResult.SeatNotAvailable:
                return Conflict("Seat is not available.");
            case BookingResult.Conflict:
                return Conflict("Seat was just taken by someone else.");
            default:
                return Ok(new { seatId = request.SeatId });
        }
    }

}
