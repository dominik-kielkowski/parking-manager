namespace ParkingManager.ParkingManager.Domain;

public class ParkingSpot
{
    public int Id { get; set; }
    public string SpotNumber { get; set; } 
    public bool IsTaken { get; set; }
    public ParkingReservation? Reservation { get; set; }
}