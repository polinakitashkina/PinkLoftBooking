namespace PinkLoftBooking.Api.Models.Domain;

public class ResourceEntity : DomainEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
