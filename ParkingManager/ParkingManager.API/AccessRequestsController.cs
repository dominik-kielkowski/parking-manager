using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Infrastructure.MediatR.Request;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("api/[controller]")]
public class AccessRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] AccessRequestCommands.CreateAccessRequestCommand command)
    {
        try
        {
            var request = await _mediator.Send(command);
            return Ok(new 
            { 
                RequestId = request.Id,
                UserId = request.User.Id,
                request.User.UserName,
                request.IsApproved
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("review")]
    public async Task<IActionResult> ReviewRequest([FromBody] AccessRequestCommands.ReviewAccessRequestCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(new { message = "Request reviewed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserRequests(int userId)
    {
        var requests = await _mediator.Send(new GetUserRequestsQuery(userId));

        return Ok(requests.Select(r => new 
        { 
            r.Id, 
            Type = r.GetType().Name, 
            r.IsApproved 
        }));
    }
}