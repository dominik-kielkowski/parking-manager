using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public static class GitHubAuthExtensions
{
    public static WebApplicationBuilder AddGitHubAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "GitHub";
        })
        .AddCookie()
        .AddGitHub(options =>
        {
            options.ClientId = builder.Configuration["GitHub:ClientId"];
            options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
            
            options.ClaimActions.MapJsonKey("urn:github:id", "id");
            options.ClaimActions.MapJsonKey("urn:github:login", "login");
            options.ClaimActions.MapJsonKey("urn:github:email", "email");
            options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
        });

        return builder;
    }
}