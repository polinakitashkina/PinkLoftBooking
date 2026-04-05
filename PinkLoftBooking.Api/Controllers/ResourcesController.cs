using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PinkLoftBooking.Api.DTOs.Resources;
using PinkLoftBooking.Api.Models.Domain;
using PinkLoftBooking.Api.Services;

namespace PinkLoftBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController(IResourceService resources) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ResourceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await resources.ListAsync(ct);
        return Ok(list);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(ResourceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateResourceRequest dto, CancellationToken ct)
    {
        var (r, error) = await resources.CreateAsync(dto, ct);
        if (error is not null) return BadRequest(new { message = error });
        return Ok(r);
    }
}
