using Microsoft.EntityFrameworkCore;
using SecureMed.Migrations;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.ServiceDefaults;
using SecureMed.SharedKernel.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Aspire defaults (logging, telemetry)
builder.AddServiceDefaults();

// builder.Services
//     .AddFusionCache("Customers")
//     .WithCacheKeyPrefixByCacheName()
//     .WithDistributedCache(new RedisCache(new RedisCacheOptions() { Configuration = builder.Configuration.GetConnectionString("cache") }))
//     .WithSerializer(new FusionCacheSystemTextJsonSerializer())
//     .WithBackplane(new RedisBackplane(new RedisBackplaneOptions() { Configuration = builder.Configuration.GetConnectionString("cache") }))
//     .AsKeyedHybridCacheByCacheName();

// OpenTelemetry setup zodat je de migratie-stappen ziet in het dashboard
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

// Todo: use sops for encryption key management
builder.Services.AddSingleton<IEncryptionService>(new AesEncryptionService("JouwGeheimeSleutelVan32Tekens!"));

// REGISTREER JOUW DB CONTEXT
builder.Services.AddDbContext<PatientCareDbContext>(options =>
{
    // Zorg dat "patientcare-db" matcht met de naam in je AppHost!
    options.UseNpgsql(builder.Configuration.GetConnectionString("securemed-db"), optionsBuilder =>
    {
        optionsBuilder.EnableRetryOnFailure();
        // Vertel EF dat de migratie-bestanden (als je ze ooit maakt) in DIT project staan
        optionsBuilder.MigrationsAssembly(typeof(DbInitializer).Assembly.GetName().Name);
    });
});

// De initializer die het echte werk doet
builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddHostedService<DbInitializer>();

builder.Services.AddHealthChecks()
    .AddCheck<DbInitializerHealthCheck>("DbInitializer", null);
var app = builder.Build();

// Handige endpoints voor tijdens het dev-werk (F5-vriendelijk)
if (app.Environment.IsDevelopment())
{
    app.MapPost("/reset-db", async (PatientCareDbContext dbContext, DbInitializer dbInitializer, CancellationToken cancellationToken) =>
    {
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbInitializer.InitializeDatabaseAsync(dbContext, cancellationToken);
        return Results.Ok("Database reset successfully");
    });
}

app.MapDefaultEndpoints();
await app.RunAsync();
