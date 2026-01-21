using System.Text.Json;
using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Infrastructure;

public class JsonParkingSpotFactory : IParkingSpotFactory
{
    private readonly string _filePath;

    public JsonParkingSpotFactory(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<ParkingSpot> CreateParkingSpots()
    {
        var json = File.ReadAllText(_filePath);

        var spots = JsonSerializer.Deserialize<List<ParkingSpot>>(json)
                    ?? new List<ParkingSpot>();

        return spots.Select(s => new ParkingSpot
        {
            SpotNumber = s.SpotNumber,
            IsTaken = false
        });
    }
}