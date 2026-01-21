using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.ParkingManager.Application;

namespace ParkingManager.ParkingManager.API;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("Callback");
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "GitHub");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        var user = await _authRepository.HandleGitHubCallbackAsync(User);
        if (user == null)
            return Unauthorized();

        return Redirect("/index.html");
    }

    [HttpGet("current")]
    public IActionResult CurrentUser()
    {
        return Ok(_authRepository.GetCurrentUser(User));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authRepository.LogoutAsync(HttpContext);
        return Ok(new { Message = "Logged out" });
    }
}