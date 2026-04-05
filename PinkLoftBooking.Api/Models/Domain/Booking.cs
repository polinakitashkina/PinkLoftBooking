namespace PinkLoftBooking.Api.Models.Domain;

public class Booking : DomainEntity
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public Guid ResourceId { get; set; }
    public ResourceEntity Resource { get; set; } = null!;

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Active;
}
