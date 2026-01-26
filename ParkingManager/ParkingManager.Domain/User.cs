namespace ParkingManager.ParkingManager.Domain;

public class User
{
    public int Id { get; set; }
    public string GitHubId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }
    public bool HasDisabledPermit { get; set; }
    public bool HasManagerAccess { get; set; }

    public ICollection<ParkingReservation> Reservations { get; set; } = new List<ParkingReservation>();
    public ICollection<AccessRequest> AccessRequests { get; set; } = new List<AccessRequest>();
}