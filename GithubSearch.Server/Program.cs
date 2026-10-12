
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GithubSearch.Server.Models;
using GithubSearch.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Read the JWT secret from configuration.
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

if (string.IsNullOrWhiteSpace(jwtSecretKey) ||
    Encoding.UTF8.GetByteCount(jwtSecretKey) < 32)
{
    throw new InvalidOperationException(
        "Configure Jwt:SecretKey with at least 32 bytes.");
}

var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSecretKey));

const string issuer = "GithubSearch.Server";
const string audience = "GithubSearch.Client";

// Allow the Angular development server.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register application services.
builder.Services.AddHttpClient<IGitHubService, GitHubService>();
builder.Services.AddSingleton<IBookmarkService, SessionBookmarkService>();

// Configure JWT authentication.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowClient");
app.UseAuthentication();
app.UseAuthorization();

// Demo login with credentials stored in configuration.
app.MapPost("/api/auth/login", (LoginRequest request) =>
{
    var expectedUsername = builder.Configuration["DemoAuth:Username"];
    var expectedPassword = builder.Configuration["DemoAuth:Password"];

    if (string.IsNullOrWhiteSpace(expectedUsername) ||
        string.IsNullOrWhiteSpace(expectedPassword))
    {
        return Results.Problem(
            "Demo login credentials are not configured.",
            statusCode: 500);
    }

    if (string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password) ||
        request.Username != expectedUsername ||
        request.Password != expectedPassword)
    {
        return Results.Unauthorized();
    }

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, expectedUsername),
        new Claim(ClaimTypes.Name, expectedUsername),
        new Claim("username", expectedUsername)
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Issuer = issuer,
        Audience = audience,
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Results.Ok(new AuthResponse
    {
        Token = tokenHandler.WriteToken(token),
        Username = expectedUsername
    });
});

// Search GitHub repositories.
app.MapGet("/api/repos/search",
    async (string query, IGitHubService gitHubService) =>
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Results.BadRequest(
                new { message = "Query parameter is required." });
        }

        var results = await gitHubService.SearchRepositoriesAsync(query);
        return Results.Ok(results);
    }).RequireAuthorization();

// Add a bookmark.
app.MapPost("/api/bookmarks",
    (RepositoryItem item, ClaimsPrincipal user,
     IBookmarkService bookmarkService) =>
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();

        bookmarkService.AddBookmark(userId, item);
        return Results.Ok(new { message = "Bookmarked successfully" });
    }).RequireAuthorization();

// Get bookmarks.
app.MapGet("/api/bookmarks",
    (ClaimsPrincipal user, IBookmarkService bookmarkService) =>
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();

        return Results.Ok(bookmarkService.GetBookmarks(userId));
    }).RequireAuthorization();

// Remove a bookmark.
app.MapDelete("/api/bookmarks/{id:long}",
    (long id, ClaimsPrincipal user,
     IBookmarkService bookmarkService) =>
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return Results.Unauthorized();

        bookmarkService.RemoveBookmark(userId, id);
        return Results.Ok(new { message = "Removed from bookmarks" });
    }).RequireAuthorization();

app.MapGet("/", () => "Backend is running successfully!");

app.Run("http://localhost:5000");
