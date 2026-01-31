using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Infrastructure.MediatR.Request;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("[controller]")]
public class AccessRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(
        [FromBody] AccessRequestCommands.CreateAccessRequestCommand command)
    {
        var request = await _mediator.Send(command);
        return Ok(new
        {
            request.Id,
            request.UserId,
            request.User.UserName,
            Type = request.GetType().Name,
            request.IsApproved
        });
    }

    [HttpPost("review")]
    public async Task<IActionResult> ReviewRequest(
        [FromBody] AccessRequestCommands.ReviewAccessRequestCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("{userId:int}")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _mediator.Send(new GetAllAccessRequestsQuery());
        return Ok(requests.Select(r => new
        {
            r.Id,
            UserName = r.User.UserName,
            Type = r.GetType().Name,
            r.IsApproved
        }));
    }
}