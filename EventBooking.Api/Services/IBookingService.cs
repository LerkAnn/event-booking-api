namespace EventBooking.Api.Services;

public interface IBookingService
{
        Task<BookingResult> CreateBookingAsync(int userId, int seatId);
}
