namespace EventBooking.Api.Services;

public enum PaymentResults
{
    Paid,
    BookingNotFound,
    NotTheOwner,
    BookingNotPending,
    Conflict
}