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

        public async Task<object> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var githubId = request.User.FindFirst("urn:github:id")?.Value;

            if (string.IsNullOrEmpty(githubId))
                return new { isAuthenticated = false };

            var dbUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.GitHubId == githubId, cancellationToken);

            if (dbUser == null)
                return new { isAuthenticated = false };

            return new
            {
                isAuthenticated = true,
                userName = dbUser.UserName,
                isAdmin = dbUser.IsAdmin,
                hasManagerAccess = dbUser.HasManagerAccess
            };
        }
        
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
