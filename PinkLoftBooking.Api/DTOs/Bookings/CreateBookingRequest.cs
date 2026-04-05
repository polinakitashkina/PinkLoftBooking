using System.ComponentModel.DataAnnotations;

namespace PinkLoftBooking.Api.DTOs.Bookings;

public class CreateBookingRequest
{
    [Required]
    public Guid ResourceId { get; set; }

    [Required]
    public DateTime StartUtc { get; set; }

    [Required]
    public DateTime EndUtc { get; set; }
}
