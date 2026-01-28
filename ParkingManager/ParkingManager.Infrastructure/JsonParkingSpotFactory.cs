using System.Text.Json;
using System.Text.Json.Serialization;
using ParkingManager.ParkingManager.Application.Interfaces;
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

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        var spots = JsonSerializer.Deserialize<List<ParkingSpot>>(json, options)
                    ?? new List<ParkingSpot>();

        // Zawsze ustawiamy IsTaken na false przy seedowaniu
        return spots.Select(s => new ParkingSpot
        {
            SpotNumber = s.SpotNumber,
            SpotType = s.SpotType,
            IsTaken = false
        });
    }
}