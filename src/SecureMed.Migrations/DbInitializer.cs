using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Migrations;

internal partial class DbInitializer(IServiceProvider serviceProvider, ILogger<DbInitializer> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var patientDbContext = scope.ServiceProvider.GetRequiredService<PatientCareDbContext>();
        using var activity = _activitySource.StartActivity("Initializing Patient database", ActivityKind.Client);

        // Initialize PatientCare database
        await InitializeDatabaseAsync(patientDbContext, cancellationToken);
    }

    public async Task InitializeDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        //Todo: after database schema is more or less stable, we can enable migrations again
        // await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);
        await strategy.ExecuteAsync(dbContext.Database.EnsureCreatedAsync, cancellationToken);


        await SeedAsync(dbContext, cancellationToken);

        LogDatabaseInitializationCompleted(logger, sw.ElapsedMilliseconds);
    }

    private async Task SeedAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        LogSeedingDatabase(logger);

        await SeedPatients(dbContext, cancellationToken);
    }

   private static async Task SeedPatients(DbContext context, CancellationToken cancellationToken)
{
    if (!context.Set<Patient>().Any())
    {
        // Merk op: we geven gewoon platte tekst mee voor het INSZ
        var patients = new List<Patient>
        {
            Patient.Create("Jan", "Janssen", "85010112345"),
            Patient.Create("Marie", "Peeters", "92051298765")
        };

        await context.Set<Patient>().AddRangeAsync(patients, cancellationToken);
        // HIER GEBEURT DE MAGIE:
        // Bij SaveChangesAsync worden de [Encrypted] en [SearchableHash] tags verwerkt!
        await context.SaveChangesAsync(cancellationToken);
    }
}
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Database initialization completed after {ElapsedMilliseconds}ms")]
    private static partial void LogDatabaseInitializationCompleted(
        ILogger logger,
        long elapsedMilliseconds);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Seeding database")]
    private static partial void LogSeedingDatabase(ILogger logger);
}
