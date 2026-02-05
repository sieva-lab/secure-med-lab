using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureMed.SharedKernel.Infrastructure.Security;

namespace SecureMed.SharedKernel;

public static class SecurityExtensions
{
    // Alleen aanroepen in modules die echt crypto nodig hebben
    public static IServiceCollection AddEncryptionServices(this IServiceCollection services, IConfiguration configuration)
    {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            // 1. Haal de sleutel op uit configuratie (appsettings.json of Aspire)
            var encryptionKey = configuration["Encryption:Key"];

            // 2. Registreer de service (Alleen als de sleutel er is, of gooi een error)
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                services.AddSingleton<IEncryptionService>(new AesEncryptionService(encryptionKey));
            }

            return services;
    }
}
