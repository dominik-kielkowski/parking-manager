using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Infrastructure;
using ParkingManager.ParkingManager.Infrastructure.MediatR.Auth;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var redirectUrl = await _mediator.Send(new LoginCommand());
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "GitHub");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        var user = await _mediator.Send(new GitHubCallbackCommand(User));
        if (user == null) return Unauthorized();

        return Redirect("/index.html");
    }

    [HttpGet("current")]
    public async Task<IActionResult> CurrentUser()
    {
        var currentUser = await _mediator.Send(new GetCurrentUserQuery(User));
        return Ok(currentUser);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand(HttpContext));
        return Ok(new { Message = "Logged out" });
    }
}