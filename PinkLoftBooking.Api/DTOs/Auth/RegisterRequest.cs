using System.ComponentModel.DataAnnotations;

namespace PinkLoftBooking.Api.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    [MaxLength(256)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Минимум 6 символов")]
    [MaxLength(200)]
    public string Password { get; set; } = "";
}
