namespace ParkingManager.ParkingManager.Domain;

public enum ParkingSpotType
{
    Regular,
    Disabled,
    Manager
}

public class ParkingSpot
{
    public int Id { get; set; }
    public string SpotNumber { get; set; } = string.Empty;
    public bool IsTaken { get; set; }
    public ParkingSpotType SpotType { get; set; } = ParkingSpotType.Regular;
    public ParkingReservation? Reservation { get; set; }
}
