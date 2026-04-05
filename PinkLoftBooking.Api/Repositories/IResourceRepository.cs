using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Repositories;

public interface IResourceRepository
{
    Task<IReadOnlyList<ResourceEntity>> ListAsync(CancellationToken ct = default);
    Task<ResourceEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(ResourceEntity resource, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
