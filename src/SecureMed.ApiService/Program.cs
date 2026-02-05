using SecureMed.ApiService;
using SecureMed.ServiceDefaults;
using SecureMed.SharedKernel.Modules;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Services Configuratie ---
builder.AddServiceDefaults();
builder.AddSecurity();
builder.AddAuthentication();
builder.AddOpenApi();
builder.AddErrorHandling();
builder.AddCaching();
builder.AddWolverine();
builder.AddModules(); // Registreert IModule services via DiscoverModules

var app = builder.Build();

// --- 2. Middleware Pipeline (De volgorde is hier cruciaal!) ---

// Altijd als eerste: Security headers en foutafhandeling
if (app.Environment.IsDevelopment())
{
    // app.UseSecurityHeaders(policies =>
    //     policies
    //         .AddFrameOptionsDeny()
    //         .AddXssProtectionDisabled()
    //         .AddContentTypeOptionsNoSniff()
    //         .AddContentSecurityPolicy(csp =>
    //         {
    //             csp.AddDefaultSrc().Self();

    //             // Scalar specifieke noden:
    //             csp.AddScriptSrc()
    //                .Self()
    //                .UnsafeInline()
    //                .UnsafeEval(); // Vaak nodig voor SPA frameworks

    //             csp.AddStyleSrc()
    //                .Self()
    //                .UnsafeInline(); // Voor de styling van de UI

    //             csp.AddImgSrc().Self().CustomSources("data:", "https://scalar.com");

    //             csp.AddConnectSrc().Self(); // Om de /openapi/v1.json op te halen

    //             // Deze directive overschrijft script-src in moderne browsers
    //             csp.AddCustomDirective("script-src-elem", "self 'unsafe-inline' 'unsafe-eval' blob:");
    //         }));
}
else
{
    app.UseSecurityHeaders();
}
// app.UseSecurityHeaders();
app.UseExceptionHandler();
app.UseStatusCodePages();

// Routing & Auth
app.UseAuthentication();
app.UseAuthorization();

// Aspire & Cloud defaults
app.MapDefaultEndpoints();

app.MapStaticAssets();
// De Modules: Hier worden de endpoints (zoals /patients) echt gemapt
app.UseModules();

// Documentatie (alleen in Dev)
if (app.Environment.IsDevelopment())
{
   app.MapOpenApi();
   app.MapScalarApiReference();
//    app.MapScalarApiReference("api-docs", options =>
//     {
//         // Gebruik de property in plaats van de methode
//         options.OpenApiRoutePattern = "/openapi/v1.json";
//         options.Title = "SecureMed API";
//     });
}

// --- 3. Sanity Checks & Start ---

var key = app.Configuration["Encryption:Key"];
// Check if we are generating docs (EF design-time or OpenAPI), exclude those scenarios
bool isGeneratingDocs = EF.IsDesignTime ||
                         AppDomain.CurrentDomain.FriendlyName.Contains("GetDocument", StringComparison.OrdinalIgnoreCase);

if (string.IsNullOrEmpty(key) && !isGeneratingDocs)
{
    throw new InvalidOperationException("CRITICAL: Encryption:Key is missing from configuration!");
}

if (!string.IsNullOrEmpty(key))
{
    Console.WriteLine($"✅ Encryption:Key geladen (Lengte: {key.Length})");
}

app.Run();
