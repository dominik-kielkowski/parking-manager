using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Infrastructure.MediatR.Parking;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("[controller]")]
public class ParkingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingController(IMediator mediator)
    {
        _mediator = mediator;
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
            var message = await _mediator.Send(new BookParkingCommand(githubId, spotNumber));
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
        var result = await _mediator.Send(new GetParkingSpotsQuery());
        return Ok(result);
    }
}