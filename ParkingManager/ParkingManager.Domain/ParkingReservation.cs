using parking_manager;

namespace ParkingManager.ParkingManager.Domain;

public class ParkingReservation
{
    public int Id { get; set; }
    public int ParkingSpotId { get; set; }
    public ParkingSpot ParkingSpot { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
}