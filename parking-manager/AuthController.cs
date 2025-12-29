using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("Callback");
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "GitHub");
    }

    [HttpGet("callback")]
    public IActionResult Callback()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/index.html");
        }

        return Unauthorized();
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}