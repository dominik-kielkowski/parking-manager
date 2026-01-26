using System.Security.Claims;
using MediatR;
using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Auth
{
    public record LoginCommand() : IRequest<string>;

    public record GitHubCallbackCommand(ClaimsPrincipal User) : IRequest<User?>;

    public record GetCurrentUserQuery(ClaimsPrincipal User) : IRequest<object>;

    public record LogoutCommand(HttpContext HttpContext) : IRequest;
}