namespace EventBooking.Api.Services;

public record CreateBookingOutcome(BookingResult Result, int? BookingId = null);