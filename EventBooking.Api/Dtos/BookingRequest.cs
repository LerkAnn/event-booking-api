using System.ComponentModel.DataAnnotations;

namespace EventBooking.Api.Dtos;

public class BookingRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "SeatId must be greater than 0")]
    public int SeatId { get; set; }
}