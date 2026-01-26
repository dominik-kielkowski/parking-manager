using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Application.Interfaces;

public interface IParkingSpotFactory
{
    IEnumerable<ParkingSpot> CreateParkingSpots();
}