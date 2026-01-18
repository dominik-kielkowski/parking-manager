using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using parking_manager;

[ApiController]
[Route("[controller]")]
public class ParkingController : ControllerBase
{
    private readonly AppDbContext _context;
    private static readonly string Path = "ParkingSpots.json";
    private static List<ParkingSpot> _parkingSpots;

    public ParkingController(AppDbContext context)
    {
        _context = context;
        
        if (System.IO.File.Exists(Path))
        {
            var json = System.IO.File.ReadAllText(Path);
            _parkingSpots = JsonSerializer.Deserialize<List<ParkingSpot>>(json);
        }
        else
        {
            _parkingSpots = new List<ParkingSpot>();
        }
    }
    
    [HttpGet("book")]
    public string BookParking(string spotNumber)
    {
        var spot = _parkingSpots.FirstOrDefault(x => x.SpotNumber == spotNumber);
        
        if (spot.IsTaken == false)
        {
            spot.IsTaken = true;
        }

        return $"parking sopt {spot.SpotNumber} has been booked";
    }

    [HttpGet("spots")]
    public IActionResult GetParkingSpotsJson()
    {
        _context.ParkingSpots.ToList();
        string path = "ParkingManager.Infrastructure/Data/ParkingSpots.json";
        List<ParkingSpot> parkingSpots = path.SeedFromJson();
    
        return Ok(parkingSpots);
    }
}