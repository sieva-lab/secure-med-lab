using Microsoft.EntityFrameworkCore;
using SecureMed.Migrations;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.ServiceDefaults;
using SecureMed.SharedKernel;
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


builder.Services.AddEncryptionServices(builder.Configuration);


builder.Services.AddDbContext<PatientCareDbContext>(options =>
{

    options.UseNpgsql(builder.Configuration.GetConnectionString("securemed-db"), optionsBuilder =>
    {
        optionsBuilder.EnableRetryOnFailure();
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
