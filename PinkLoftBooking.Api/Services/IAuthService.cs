using PinkLoftBooking.Api.DTOs.Auth;

namespace PinkLoftBooking.Api.Services;

public interface IAuthService
{
    Task<(AuthResponse? response, string? error)> RegisterAsync(RegisterRequest dto, CancellationToken ct = default);
    Task<(AuthResponse? response, string? error)> LoginAsync(LoginRequest dto, CancellationToken ct = default);
}
