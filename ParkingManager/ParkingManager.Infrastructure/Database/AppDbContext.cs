using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ParkingSpot> ParkingSpots => Set<ParkingSpot>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ParkingReservation> ParkingReservations => Set<ParkingReservation>();
    public DbSet<AccessRequest> AccessRequests => Set<AccessRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AccessRequest>()
            .HasKey(ar => ar.Id);

        modelBuilder.Entity<AccessRequest>()
            .HasDiscriminator<string>("AccessRequestType")
            .HasValue<DisabledPermitRequest>("DisabledPermit")
            .HasValue<ManagerAccessRequest>("Manager");


        modelBuilder.Entity<ParkingSpot>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<ParkingReservation>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<ParkingReservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<ParkingReservation>()
            .HasOne(r => r.ParkingSpot)
            .WithOne(s => s.Reservation)
            .HasForeignKey<ParkingReservation>(r => r.ParkingSpotId);
        
        modelBuilder.Entity<AccessRequest>()
            .HasOne(ar => ar.User)
            .WithMany(u => u.AccessRequests)
            .HasForeignKey(ar => ar.UserId)
            .IsRequired();
    }
}