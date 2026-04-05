using System.ComponentModel.DataAnnotations;

namespace PinkLoftBooking.Api.DTOs.Resources;

public class CreateResourceRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [MaxLength(2000)]
    public string Description { get; set; } = "";
}
