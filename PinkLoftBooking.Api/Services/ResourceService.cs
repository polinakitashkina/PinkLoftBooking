using PinkLoftBooking.Api.DTOs.Resources;
using PinkLoftBooking.Api.Models.Domain;
using PinkLoftBooking.Api.Repositories;

namespace PinkLoftBooking.Api.Services;

public class ResourceService(IResourceRepository resources, ILogger<ResourceService> logger) : IResourceService
{
    public async Task<IReadOnlyList<ResourceResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await resources.ListAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<(ResourceResponse? r, string? error)> CreateAsync(CreateResourceRequest dto, CancellationToken ct = default)
    {
        var entity = new ResourceEntity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim()
        };
        await resources.AddAsync(entity, ct);
        await resources.SaveChangesAsync(ct);
        logger.LogInformation("Создан ресурс {Name} ({Id})", entity.Name, entity.Id);
        return (Map(entity), null);
    }

    private static ResourceResponse Map(ResourceEntity e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Description = e.Description
    };
}
