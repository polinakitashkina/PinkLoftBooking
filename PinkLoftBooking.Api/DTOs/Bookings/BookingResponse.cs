using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.DTOs.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; }
    public string ResourceName { get; set; } = "";
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public BookingStatus Status { get; set; }
}
