namespace PinkLoftBooking.Api.Models.Domain;

public class AppUser : DomainEntity
{
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.User;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
