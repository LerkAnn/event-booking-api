namespace EventBooking.Api.Services;

public interface IBookingService
{
    Task<CreateBookingOutcome> CreateBookingAsync(int userId, int seatId);
    Task<PaymentResults> PayBookingAsync(int userId, int bookingId);
    Task<int> ExpireHoldsAsync(TimeSpan maxAge);
}
