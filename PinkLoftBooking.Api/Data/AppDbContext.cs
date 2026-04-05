using Microsoft.EntityFrameworkCore;
using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ResourceEntity> Resources => Set<ResourceEntity>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(256).IsRequired();
        });

        modelBuilder.Entity<ResourceEntity>(e =>
        {
            e.Property(r => r.Name).HasMaxLength(200).IsRequired();
            e.Property(r => r.Description).HasMaxLength(2000);
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(b => b.Resource).WithMany(r => r.Bookings).HasForeignKey(b => b.ResourceId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(b => new { b.ResourceId, b.Status, b.StartUtc, b.EndUtc });
        });
    }
}
