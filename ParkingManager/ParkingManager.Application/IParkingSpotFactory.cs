using parking_manager;

public interface IParkingSpotFactory
{
    IEnumerable<ParkingSpot> CreateParkingSpots();
}