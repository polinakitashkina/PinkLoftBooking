using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = "";
    public Guid UserId { get; set; }
    public string Email { get; set; } = "";
    public UserRole Role { get; set; }
}
