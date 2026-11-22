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
        var outcome = await _bookingService.CreateBookingAsync(userId, request.SeatId);
        switch (outcome.Result)
        {
            case BookingResult.SeatNotFound:
                return NotFound("Seat not found.");
            case BookingResult.SeatNotAvailable:
                return Conflict("Seat is not available.");
            case BookingResult.Conflict:
                return Conflict("Seat was just taken by someone else.");
            default:
                return Ok(new { bookingId = outcome.BookingId, seatId = request.SeatId });
        }
    }

    [Authorize]
    [HttpPost("{id}/pay")]
    public async Task<IActionResult> Pay(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _bookingService.PayBookingAsync(userId, id);
        switch (result)
        {
            case PaymentResults.BookingNotFound:
                return NotFound("Booking not found.");
            case PaymentResults.NotTheOwner:
                return NotFound("Booking not found.");
            case PaymentResults.Conflict:
                return Conflict("Booking got conflict, please try again.");
            case PaymentResults.BookingNotPending:
                return Conflict("The booking is not pending for payment");
            default:
                return Ok("Seat is paid.");
        }
    }
}
