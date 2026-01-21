using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Application;
using ParkingManager.ParkingManager.Infrastructure;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("[controller]")]
public class ParkingController : ControllerBase
{
    private readonly IParkingRepository _repository;

    public ParkingController(IParkingRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookParking([FromQuery] string spotNumber)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Unauthorized();

        var githubId = User.FindFirst("urn:github:id")?.Value;
        if (githubId == null) return BadRequest("GitHub ID missing");

        try
        {
            var message = await _repository.BookSpotAsync(githubId, spotNumber);
            return Ok(new { Message = message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("spots")]
    public async Task<IActionResult> GetParkingSpotsJson()
    {
        var result = await _repository.GetSpotsStatusAsync();
        return Ok(result);
    }
}