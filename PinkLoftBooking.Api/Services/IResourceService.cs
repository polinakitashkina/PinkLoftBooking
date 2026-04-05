using PinkLoftBooking.Api.DTOs.Resources;

namespace PinkLoftBooking.Api.Services;

public interface IResourceService
{
    Task<IReadOnlyList<ResourceResponse>> ListAsync(CancellationToken ct = default);
    Task<(ResourceResponse? r, string? error)> CreateAsync(CreateResourceRequest dto, CancellationToken ct = default);
}
