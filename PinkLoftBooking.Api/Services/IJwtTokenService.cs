using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Services;

public interface IJwtTokenService
{
    string CreateToken(AppUser user);
}
