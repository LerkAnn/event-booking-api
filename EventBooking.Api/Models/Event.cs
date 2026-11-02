using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Api.Models;

[Table("Events")]
public class Event
{
    [Column("event_id")]
    public int EventId { get; set; }

    [Required, StringLength(200)]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "TotalSeats must be greater than 0")]
    [Column("total_seats")]
    public int TotalSeats { get; set; }
}