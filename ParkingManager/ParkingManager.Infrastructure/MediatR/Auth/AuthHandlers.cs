using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Domain;
using ParkingManager.ParkingManager.Infrastructure.Database;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Auth
{
    public class AuthHandlers :
        IRequestHandler<LoginCommand, string>,
        IRequestHandler<GitHubCallbackCommand, User?>,
        IRequestHandler<GetCurrentUserQuery, object>,
        IRequestHandler<LogoutCommand>
    {
        private readonly AppDbContext _context;

        public AuthHandlers(AppDbContext context)
        {
            _context = context;
        }

        public Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // In the original controller, redirect URL is generated from Url.Action
            // For simplicity, let's return a fixed callback path; the controller can wrap it
            return Task.FromResult("/auth/callback");
        }

        public async Task<User?> Handle(GitHubCallbackCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            if (!(user.Identity?.IsAuthenticated ?? false))
                return null;

            var githubId = user.FindFirst("urn:github:id")?.Value;
            var githubLogin = user.FindFirst("urn:github:login")?.Value;

            if (githubId == null || githubLogin == null)
                throw new InvalidOperationException("Missing GitHub user info");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.GitHubId == githubId, cancellationToken);
            if (existingUser != null) return existingUser;

            var newUser = new User
            {
                UserName = githubLogin,
                GitHubId = githubId
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return newUser;
        }

        public Task<object> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = request.User;

            if (!(user.Identity?.IsAuthenticated ?? false))
                return Task.FromResult<object>(new { IsAuthenticated = false });

            return Task.FromResult<object>(new
            {
                IsAuthenticated = true,
                UserName = user.Identity.Name
            });
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
