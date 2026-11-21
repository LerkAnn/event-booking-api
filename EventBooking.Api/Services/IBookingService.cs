namespace EventBooking.Api.Services;

public interface IBookingService
{
    Task<BookingResult> CreateBookingAsync(int userId, int seatId);
    Task<PaymentResults> PayBookingAsync(int userId, int bookingId);

}
