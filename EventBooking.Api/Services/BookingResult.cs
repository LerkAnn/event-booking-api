namespace EventBooking.Api.Services;

public enum BookingResult
{
    Created,
    SeatNotFound,
    SeatNotAvailable,
    Conflict
}