using parking_manager;
using ParkingManager.ParkingManager.Domain;

public interface IParkingSpotFactory
{
    IEnumerable<ParkingSpot> CreateParkingSpots();
}