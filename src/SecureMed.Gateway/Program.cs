using Duende.AccessTokenManagement.OpenIdConnect;
using SecureMed.Gateway;
using SecureMed.Gateway.UserModule;
using SecureMed.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

builder.AddServiceDefaults();
builder.AddAuthenticationSchemes();
builder.AddRateLimiting();
builder.AddReverseProxy();

builder.Services.AddDistributedMemoryCache();

// OpenID Connect access token management
// This is used to acquire and manage access tokens for the authenticated user
builder.Services.AddOpenIdConnectAccessTokenManagement();

// Antiforgery configuration
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    // Set SameSite attributes for the antiforgery cookie
    // Strict is used here since the gateway and backend APIs are assumed to be on the same site
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

//  Antiforgery middleware has to be placed before authentication middleware
app.UseAntiforgery();

app.UseAuthentication();

// Rate limiting middleware to limit requests based on user identity or IP address
app.UseRateLimiter();
app.UseAuthorization();

// Reverse proxy middleware
app.MapGroup("bff")
    .MapUserEndpoints();

// Map reverse proxy routes
app.MapReverseProxy();

app.MapDefaultEndpoints();

app.Run();
