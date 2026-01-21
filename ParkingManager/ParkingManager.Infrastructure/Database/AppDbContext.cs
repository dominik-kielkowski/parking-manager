using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<ParkingSpot> ParkingSpots => Set<ParkingSpot>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ParkingReservation> ParkingReservations => Set<ParkingReservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ParkingSpot>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<ParkingReservation>()
            .HasKey(r => r.Id);

        // Relationships
        modelBuilder.Entity<ParkingReservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<ParkingReservation>()
            .HasOne(r => r.ParkingSpot)
            .WithOne(s => s.Reservation)
            .HasForeignKey<ParkingReservation>(r => r.ParkingSpotId);
    }
}