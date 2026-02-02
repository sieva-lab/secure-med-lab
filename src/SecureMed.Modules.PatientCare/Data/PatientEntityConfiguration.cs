using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureMed.Modules.PatientCare.Domain;

namespace SecureMed.Modules.PatientCare.Data;

internal class PatientEntityConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients");
        builder.HasKey(p => p.Id);

        builder.ComplexProperty(p => p.Name, o =>
        {
            o.Property(p => p.FirstName).HasMaxLength(255);
            o.Property(p => p.LastName).HasMaxLength(255);
        });

        // De versleutelde kolom (de [Encrypted] tag in de entiteit regelt de logica,
        // maar hier bepalen we de database-instellingen)
        builder.Property(p => p.Insz)
            .IsRequired()
            .HasMaxLength(500); // Ruimte geven voor de encryptie overhead + Base64

        // De doorzoekbare hash kolom
        builder.Property(p => p.InszHash)
            .IsRequired()
            .HasMaxLength(255);

        // INDEXERING: Maak de hash uniek zodat een patiënt niet twee keer
        // met hetzelfde rijksregisternummer kan worden aangemaakt.
        builder.HasIndex(p => p.InszHash)
            .IsUnique()
            .HasDatabaseName("ix_patients_insz_hash");

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.CreatedBy).HasMaxLength(255);


    }
}
