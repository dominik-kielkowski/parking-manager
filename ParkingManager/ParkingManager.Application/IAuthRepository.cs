using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Application;

public interface IAuthRepository
{
    Task<User?> HandleGitHubCallbackAsync(System.Security.Claims.ClaimsPrincipal user);
    object GetCurrentUser(System.Security.Claims.ClaimsPrincipal user);
    Task LogoutAsync(HttpContext httpContext);
}