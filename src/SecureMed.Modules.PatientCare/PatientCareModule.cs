using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.Modules.PatientCare.Application;
using SecureMed.SharedKernel.Modules;
using Wolverine.Attributes;
using ZiggyCreatures.Caching.Fusion;

[assembly: WolverineModule] // Makes Wolverine scan this assembly for handlers
namespace SecureMed.Modules.PatientCare;

public class PatientCareModule : IModule
{
    // Module setup during application building
    public WebApplicationBuilder AddModule(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // 1. JSON Configuratie (Strict)
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
        });

        // 2. Database Context (Postgres)
        builder.Services.AddDbContext<PatientCareDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("securemed-db"));
        });

        // 3. Direct IQueryable Injection for Patient entity
        // We register the DbSet as an IQueryable with AsNoTracking for fast reads
        builder.Services.AddTransient(sp =>
            sp.GetRequiredService<PatientCareDbContext>()
              .Set<Patient>()
              .AsNoTracking());

        // 4. FusionCache / HybridCache setup
        // ZiggyCreatures FusionCache for caching with HybridCache support
        builder.Services.AddFusionCache("PatientCare")
            .TryWithAutoSetup()
            .WithCacheKeyPrefixByCacheName()
            .AsKeyedHybridCacheByCacheName();

        return builder;
    }

    // Module setup during application use
    public WebApplication UseModule(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // 5. Endpoint Routing (Vertical Slices)
        var group = app.MapGroup("patients")
          //  .RequireAuthorization() // HIPAA/GDPR eis
            .WithTags("Patient Care");

        group.MapGet("{patientId}", GetPatient.Query)
            .WithName("GetPatient")
            .WithDescription("Get a specific patient by ID");

        // group.MapPost("", RegisterPatient.Handle)
        //     .WithName("RegisterPatient")
        //     .WithDescription("Register a new patient into the system");


        // group.MapGet("", GetPatients.Query);
        // group.MapDelete("{patientId}", DeletePatient.Handle);

        return app;
    }
}
