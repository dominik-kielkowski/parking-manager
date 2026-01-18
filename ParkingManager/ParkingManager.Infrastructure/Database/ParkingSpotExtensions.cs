using System.Text.Json;

namespace parking_manager
{
    public static class ParkingSpotExtensions
    {
        public static List<ParkingSpot> SeedFromJson(this string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"JSON file not found: {filePath}");

            var json = File.ReadAllText(filePath);
            var spots = JsonSerializer.Deserialize<List<ParkingSpot>>(json);

            return spots ?? new List<ParkingSpot>();
        }
    }
}