namespace ParkingManager.ParkingManager.Domain;

public abstract class AccessRequest
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public bool IsApproved { get; set; } = false;
    public abstract void Apply();
}

public class DisabledPermitRequest : AccessRequest
{
    public override void Apply() => User.HasDisabledPermit = IsApproved;
}

public class ManagerAccessRequest : AccessRequest
{
    public override void Apply() => User.HasManagerAccess = IsApproved;
}
