using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("Seats")]
public class Seat
{
    [Column("seat_id")]
    public int SeatId { get; set; }

    [Column("event_id")]
    public int EventId { get; set; }

    [Column("row_label")]
    public string RowLabel { get; set; } = "";

    [Column("seat_number")]
    public int SeatNumber { get; set; }

    [Column("seat_status_id")]
    public int SeatStatusId { get; set; }
}