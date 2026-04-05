using Microsoft.EntityFrameworkCore;
using PinkLoftBooking.Api.Data;
using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Repositories;

public class ResourceRepository : RepositoryBase, IResourceRepository
{
    public ResourceRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<ResourceEntity>> ListAsync(CancellationToken ct = default) =>
        await Context.Resources.AsNoTracking().OrderBy(r => r.Name).ToListAsync(ct);

    public Task<ResourceEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Context.Resources.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(ResourceEntity resource, CancellationToken ct = default) =>
        await Context.Resources.AddAsync(resource, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => Context.SaveChangesAsync(ct);
}
