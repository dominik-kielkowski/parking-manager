using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Application;
using ParkingManager.ParkingManager.Domain;
using ParkingManager.ParkingManager.Infrastructure.Database;

namespace ParkingManager.ParkingManager.Infrastructure;

public class ParkingRepository : IParkingRepository
{
    private readonly AppDbContext _context;

    public ParkingRepository(AppDbContext context)
    {
        _context = context;
    }

    // Book a parking spot
    public async Task<string> BookSpotAsync(string githubId, string spotNumber)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.GitHubId == githubId);
        if (user == null) throw new InvalidOperationException("User not found");

        var spot = await _context.ParkingSpots
            .Include(s => s.Reservation)
            .FirstOrDefaultAsync(s => s.SpotNumber == spotNumber);

        if (spot == null) throw new InvalidOperationException("Parking spot not found");
        if (spot.Reservation != null) throw new InvalidOperationException("Spot already booked");

        var reservation = new ParkingReservation
        {
            ParkingSpotId = spot.Id,
            UserId = user.Id,
            ReservedAt = DateTime.UtcNow
        };

        _context.ParkingReservations.Add(reservation);
        await _context.SaveChangesAsync();

        return $"Spot {spot.SpotNumber} booked for {user.UserName}";
    }

    // Get all spots with availability
    public async Task<IEnumerable<object>> GetSpotsStatusAsync()
    {
        var spots = await _context.ParkingSpots
            .Include(s => s.Reservation)
            .ToListAsync();

        return spots.Select(s => new
        {
            s.SpotNumber,
            IsTaken = s.Reservation != null
        });
    }
}