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

    public async Task<PaymentResults> PayBookingAsync(int userId, int bookingId)
    {
        var booking = await _db.Bookings.FindAsync(bookingId);
        if (booking == null)
        {
            return PaymentResults.BookingNotFound;
        }
        var pendingPayment = await _db.BookingStatuses.FirstOrDefaultAsync(s => s.Code == "PENDING_PAYMENT");

        if (booking.UserId != userId)
        {
            return PaymentResults.NotTheOwner;
        }
        if(booking.BookingStatusId != pendingPayment.BookingStatusId)
        {
            return PaymentResults.BookingNotPending;
        }

        var confirmedPayment = await _db.BookingStatuses.FirstOrDefaultAsync(b => b.Code == "CONFIRMED");
        var booked = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "BOOKED");
        booking.BookingStatusId = confirmedPayment.BookingStatusId;
        booking.UpdatedAt = DateTime.UtcNow;
        var seat = await _db.Seats.FindAsync(booking.SeatId);
        seat.SeatStatusId = booked.SeatStatusId;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return PaymentResults.Conflict;
        }

        return PaymentResults.Paid;
    }
}