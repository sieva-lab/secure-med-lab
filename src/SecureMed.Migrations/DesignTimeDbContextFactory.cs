using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.SharedKernel.Infrastructure.Security;

namespace SecureMed.Migrations;

/// <summary>
/// Provides a design-time factory so the EF tools can create the DbContext
/// without requiring the migrations assembly to be copied into another project's output.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PatientCareDbContext>
{
    public PatientCareDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("securemed-db")
                               ?? Environment.GetEnvironmentVariable("MIGRATIONS_CONNECTION")
                               ?? "Host=localhost;Database=securemed;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<PatientCareDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.GetName().Name)
        );

        // Provide the runtime dependencies required by the PatientCareDbContext constructor.
        // Use the system TimeProvider and a simple AesEncryptionService instance for design-time.
        var timeProvider = TimeProvider.System;
        var encryptionKey = Environment.GetEnvironmentVariable("MIGRATIONS_ENCRYPTION_KEY") ?? "dev-migrations-key";
        var encryptionService = new AesEncryptionService(encryptionKey);

        return new PatientCareDbContext(optionsBuilder.Options, timeProvider, encryptionService);
    }
}
