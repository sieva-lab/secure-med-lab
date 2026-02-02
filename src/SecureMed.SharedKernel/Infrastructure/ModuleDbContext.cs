using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SecureMed.SharedKernel.Domain;
using SecureMed.SharedKernel.Infrastructure.Persistence;
using SecureMed.SharedKernel.Infrastructure.Security;
using SecureMed.SharedKernel.Attributes;

namespace SecureMed.SharedKernel.Infrastructure;

public abstract class ModuleDbContext(
    DbContextOptions options,
    TimeProvider timeProvider,
    IEncryptionService encryptionService) : DbContext(options)
{
    private static readonly MethodInfo s_setGlobalQueryForSoftDeleteMethod =
        typeof(ModuleDbContext).GetMethod(nameof(SetGlobalQueryForSoftDelete), BindingFlags.NonPublic | BindingFlags.Static)!;

    public abstract string Schema { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor(timeProvider));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

        // 1. Scan de assembly van de specifieke module voor IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // 2. Pas de [Encrypted] tag scan toe
        modelBuilder.UseEncryption(encryptionService);

        // 3. Bestaande SoftDelete logica
        ConfigureSoftDeleteQueryFilters(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Vogen / StronglyTypedId configuratie
        configurationBuilder.ApplyStronglyTypedIdEfConvertersFromAssembly(typeof(ModuleDbContext).Assembly);
        base.ConfigureConventions(configurationBuilder);
    }

    private void ConfigureSoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var method = s_setGlobalQueryForSoftDeleteMethod.MakeGenericMethod(entityType.ClrType);
                method.Invoke(this, [modelBuilder]);
            }
        }
    }

    private static void SetGlobalQueryForSoftDelete<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
    {
        // Gebruik Constants uit je SharedKernel
        modelBuilder.Entity<T>().HasQueryFilter(e => e.DeletedAt == null);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var properties = entry.Metadata.GetProperties()
                .Where(p => p.PropertyInfo?.GetCustomAttribute<SearchableHashAttribute>() != null);

            foreach (var prop in properties)
            {
                var attr = prop.PropertyInfo!.GetCustomAttribute<SearchableHashAttribute>()!;

                // Haal de waarde op van de bron (bijv. Insz)
                var sourceValue = entry.CurrentValues[attr.TargetProperty]?.ToString();

                if (!string.IsNullOrEmpty(sourceValue))
                {
                    // Zet de hash op de doel-property (bijv. InszHash)
                    entry.CurrentValues[prop.Name] = encryptionService.CreateHash(sourceValue);
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
