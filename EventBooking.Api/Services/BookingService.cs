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

    public async Task<CreateBookingOutcome> CreateBookingAsync(int userId, int seatId)
    {
        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null)
        {
            return new CreateBookingOutcome(BookingResult.SeatNotFound);
        }
        var available = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "AVAILABLE");

        if (seat.SeatStatusId != available.SeatStatusId)
        {
            return new CreateBookingOutcome(BookingResult.SeatNotAvailable);
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
            return new CreateBookingOutcome(BookingResult.Conflict);
        }

        return new CreateBookingOutcome(BookingResult.Created, booking.BookingId);
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
        if (booking.BookingStatusId != pendingPayment.BookingStatusId)
        {
            return PaymentResults.BookingNotPending;
        }

        var confirmedPayment = await _db.BookingStatuses.FirstOrDefaultAsync(b => b.Code == "CONFIRMED");
        var booked = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "BOOKED");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var rows = await _db.Bookings
            .Where(b => b.BookingId == bookingId && b.BookingStatusId == pendingPayment.BookingStatusId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.BookingStatusId, confirmedPayment.BookingStatusId)
                .SetProperty(b => b.UpdatedAt, DateTime.UtcNow));

        if (rows == 0)
        {
            return PaymentResults.BookingNotPending;
        }

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
        await tx.CommitAsync();
        return PaymentResults.Paid;
    }

    public async Task<int> ExpireHoldsAsync(TimeSpan maxAge)
    {
        var pendingPayment = await _db.BookingStatuses.FirstOrDefaultAsync(s => s.Code == "PENDING_PAYMENT");
        var expired = await _db.BookingStatuses.FirstOrDefaultAsync(s => s.Code == "EXPIRED");
        var available = await _db.SeatStatuses.FirstOrDefaultAsync(s => s.Code == "AVAILABLE");
        var cutoff = DateTime.UtcNow - maxAge;

        var bookings = await _db.Bookings
            .Where(b => b.BookingStatusId == pendingPayment!.BookingStatusId
                     && b.CreatedAt < cutoff)
            .ToListAsync();

        foreach (var booking in bookings)
        {
            booking.BookingStatusId = expired!.BookingStatusId;
            booking.UpdatedAt = DateTime.UtcNow;

            var seat = await _db.Seats.FindAsync(booking.SeatId);
            seat!.SeatStatusId = available!.SeatStatusId;
        }

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return 0;
        }

        return bookings.Count;
    }
}