using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Application;
using ParkingManager.ParkingManager.Domain;
using ParkingManager.ParkingManager.Infrastructure.Database;

namespace ParkingManager.ParkingManager.Infrastructure;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> HandleGitHubCallbackAsync(System.Security.Claims.ClaimsPrincipal user)
    {
        if (!(user.Identity?.IsAuthenticated ?? false))
            return null;

        var githubId = user.FindFirst("urn:github:id")?.Value;
        var githubLogin = user.FindFirst("urn:github:login")?.Value;

        if (githubId == null || githubLogin == null)
            throw new InvalidOperationException("Missing GitHub user info");

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.GitHubId == githubId);

        if (existingUser != null) return existingUser;

        var newUser = new User
        {
            UserName = githubLogin,
            GitHubId = githubId
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public object GetCurrentUser(System.Security.Claims.ClaimsPrincipal user)
    {
        if (!(user.Identity?.IsAuthenticated ?? false))
            return new { IsAuthenticated = false };

        return new
        {
            IsAuthenticated = true,
            UserName = user.Identity.Name
        };
    }

    public async Task LogoutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}