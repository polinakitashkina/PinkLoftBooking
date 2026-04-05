using Microsoft.EntityFrameworkCore;
using PinkLoftBooking.Api.Data;
using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Repositories;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        Context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task AddAsync(AppUser user, CancellationToken ct = default)
    {
        await Context.Users.AddAsync(user, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Context.SaveChangesAsync(ct);
}
