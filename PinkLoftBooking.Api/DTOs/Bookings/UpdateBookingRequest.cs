using System.ComponentModel.DataAnnotations;

namespace PinkLoftBooking.Api.DTOs.Bookings;

public class UpdateBookingRequest
{
    [Required]
    public DateTime StartUtc { get; set; }

    [Required]
    public DateTime EndUtc { get; set; }
}
