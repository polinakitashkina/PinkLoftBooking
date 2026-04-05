using PinkLoftBooking.Api.DTOs.Auth;
using PinkLoftBooking.Api.Models.Domain;
using PinkLoftBooking.Api.Repositories;

namespace PinkLoftBooking.Api.Services;

public class AuthService(
    IUserRepository users,
    IJwtTokenService jwt,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<(AuthResponse? response, string? error)> RegisterAsync(RegisterRequest dto, CancellationToken ct = default)
    {
        var email = NormalizeEmail(dto.Email);
        if (await users.GetByEmailAsync(email, ct) is not null)
            return (null, "Этот email уже зарегистрирован 💌");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.User
        };
        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);
        logger.LogInformation("Регистрация пользователя {Email}", email);

        var response = new AuthResponse
        {
            Token = jwt.CreateToken(user),
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
        return (response, null);
    }

    public async Task<(AuthResponse? response, string? error)> LoginAsync(LoginRequest dto, CancellationToken ct = default)
    {
        var email = NormalizeEmail(dto.Email);
        var user = await users.GetByEmailAsync(email, ct);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            logger.LogWarning("Неудачный вход для {Email}", email);
            return (null, "Неверный email или пароль");
        }

        logger.LogInformation("Вход пользователя {Email}", email);
        return (new AuthResponse
        {
            Token = jwt.CreateToken(user),
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        }, null);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
