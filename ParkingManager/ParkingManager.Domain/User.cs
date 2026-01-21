namespace ParkingManager.ParkingManager.Domain;

public class User
{
    public int Id { get; set; }
    public string GitHubId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public ICollection<ParkingReservation> Reservations { get; set; } = new List<ParkingReservation>();
}