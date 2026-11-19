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

        var pendingPayment = await _db.BookingStatuses.FirstOrDefaultAsync(b => b.Code == "PENDING_PAYMENT");
        var held = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "HELD");

        var booking = new Booking
        {
            UserId = userId,
            SeatId = seat.SeatId,
            BookingStatusId = pendingPayment.BookingStatusId
        };
        _db.Bookings.Add(booking);
        seat.SeatStatusId = held.SeatStatusId;

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