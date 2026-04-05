using Microsoft.AspNetCore.Mvc;
using PinkLoftBooking.Api.DTOs.Auth;
using PinkLoftBooking.Api.Services;

namespace PinkLoftBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto, CancellationToken ct)
    {
        var (response, error) = await auth.RegisterAsync(dto, ct);
        if (error is not null) return BadRequest(new { message = error });
        return Ok(response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto, CancellationToken ct)
    {
        var (response, error) = await auth.LoginAsync(dto, ct);
        if (error is not null) return BadRequest(new { message = error });
        return Ok(response);
    }
}
