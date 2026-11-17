using EventBooking.Api.Data;
using EventBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBooking.Api.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BookingResult> CreateBookingAsync(int userId, int seatId)
    {
        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null)
        {
            return BookingResult.SeatNotFound;
        }
        var available = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "AVAILABLE");

        if (seat.SeatStatusId != available.SeatStatusId)
        {
            return BookingResult.SeatNotAvailable;
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
            return BookingResult.Conflict;
        }

        return BookingResult.Created;
    }
}