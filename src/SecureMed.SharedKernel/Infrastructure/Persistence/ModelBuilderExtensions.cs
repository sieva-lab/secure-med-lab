using Microsoft.EntityFrameworkCore;
using SecureMed.SharedKernel.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SecureMed.SharedKernel.Infrastructure;
using SecureMed.SharedKernel.Infrastructure.Security;

namespace SecureMed.SharedKernel.Infrastructure.Persistence;
public static class ModelBuilderExtensions
{
    public static void UseEncryption(this ModelBuilder modelBuilder, IEncryptionService encryptionService)
    {

        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(encryptionService);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                // Check of de property onze [Encrypted] tag heeft
                if (Attribute.IsDefined(property.PropertyInfo!, typeof(EncryptedAttribute)))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<string, string>(
                        v => encryptionService.Encrypt(v),
                        v => encryptionService.Decrypt(v)
                    ));
                }
            }
        }
    }
}
