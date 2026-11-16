using EventBooking.Api.Data;
using EventBooking.Api.Dtos;
using EventBooking.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;

namespace EventBooking.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;
    public BookingsController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var seat = await _db.Seats.FindAsync(request.SeatId);
        if (seat == null)
        {
            return NotFound("Seat not found.");
        }
        var available = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "AVAILABLE");

        if (seat.SeatStatusId != available.SeatStatusId)
        {
            return Conflict("Seat is not available.");
        }

        var confirmed = await _db.BookingStatuses.FirstOrDefaultAsync(b => b.Code == "CONFIRMED");
        var booked = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "BOOKED");

        var booking = new Booking
        {
            UserId = userId,
            SeatId = seat.SeatId,
            BookingStatusId = confirmed.BookingStatusId
        };
        _db.Bookings.Add(booking);
        seat.SeatStatusId = booked.SeatStatusId;
        
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Seat was just taken by someone else.");
        }

        return Ok(new { booking.BookingId, booking.SeatId });
    }

}
