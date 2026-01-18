using Microsoft.EntityFrameworkCore;

namespace parking_manager;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<ParkingSpot> ParkingSpots => Set<ParkingSpot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSpot>(entity =>
        { 
            entity.HasKey(p => p.Id); 
            entity.Property(p => p.SpotNumber).IsRequired();
        });
    }
}